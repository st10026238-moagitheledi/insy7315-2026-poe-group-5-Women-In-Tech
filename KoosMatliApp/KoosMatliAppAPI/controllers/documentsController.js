'use strict';

const yauzl = require('yauzl');
const path = require('node:path');
const fs = require('node:fs/promises');
const { randomUUID } = require('node:crypto');
const { TextDecoder } = require('node:util');
const { bad, recordOr404 } = require('./responseHelpers');
const types = { '.pdf': 'application/pdf', '.docx': 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', '.txt': 'text/plain', '.png': 'image/png', '.jpg': 'image/jpeg', '.jpeg': 'image/jpeg' };
function isDocx(buffer) {
  return new Promise(resolve => {
    yauzl.fromBuffer(buffer, { lazyEntries: true, autoClose: true }, (error, zip) => {
      if (error) return resolve(false);
      let found = false, count = 0, done = false;
      const finish = result => { if (!done) { done = true; zip.close(); resolve(result); } };
      zip.on('error', () => finish(false));
      zip.on('entry', entry => {
        if (++count > 10000) return finish(false);
        if (entry.fileName === 'word/document.xml') found = true;
        zip.readEntry();
      });
      zip.on('end', () => finish(found)); zip.readEntry();
    });
  });
}
async function validFile(buffer, extension) {
  if (!buffer.length) return false;
  switch (extension) {
    case '.pdf': return buffer.subarray(0, 5).equals(Buffer.from('%PDF-'));
    case '.png': return buffer.subarray(0, 8).equals(Buffer.from([137, 80, 78, 71, 13, 10, 26, 10]));
    case '.jpg': case '.jpeg': return buffer.subarray(0, 3).equals(Buffer.from([255, 216, 255]));
    case '.docx': return isDocx(buffer);
    case '.txt': try { new TextDecoder('utf-8', { fatal: true }).decode(buffer); return !buffer.includes(0); } catch { return false; }
    default: return false;
  }
}

function createDocumentsController({ db, store, uploadDir }) {

  const getDocuments = async (req, res) => res.json(await store.all('schoolDocuments'));
  const getDocument = async (req, res) => recordOr404(req, res, await store.get('schoolDocuments', req.params.id));
  const uploadDocument = async (req, res) => {
    const file = req.file, name = file && path.posix.basename(file.originalname.replaceAll('\\', '/'));
    const extension = path.extname(name || '').toLowerCase(), title = req.body.title;
    if (!file || !file.size || !types[extension] || name.length > 180 || (title != null && (typeof title !== 'string' || title.length > 180))) return res.status(400).json({ message: 'Choose a PDF, DOCX, TXT, PNG or JPG file up to 10 MB.' });
    if (!(await validFile(file.buffer, extension))) return res.status(400).json({ message: 'The file content does not match its extension.' });
    await fs.mkdir(uploadDir, { recursive: true });
    const storedName = randomUUID().replaceAll('-', '') + extension, destination = path.join(uploadDir, storedName);
    let item, complete = false;
    try {
      await fs.writeFile(destination, file.buffer, { flag: 'wx' });
      item = { id: 0, title: title?.trim() || path.basename(name, extension), fileName: name, category: 'General', uploadedDate: new Date().toISOString(), fileUrl: '' };
      await store.add('schoolDocuments', item);
      item.fileUrl = `/api/admin/documents/${item.id}/file`;
      await db.collection('schoolDocumentFiles').doc(String(item.id)).set({ StoredName: storedName, ContentType: types[extension] });
      await store.save('schoolDocuments', item); complete = true;
      res.location(`/api/admin/documents/${item.id}`).status(201).json(item);
    } finally {
      if (!complete) {
        await fs.rm(destination, { force: true });
        if (item?.id) {
          await db.collection('schoolDocuments').doc(String(item.id)).delete();
          await db.collection('schoolDocumentFiles').doc(String(item.id)).delete();
        }
      }
    }
  };
  const downloadDocument = async (req, res, next) => {
    const [item, mapping] = await Promise.all([store.get('schoolDocuments', req.params.id), db.collection('schoolDocumentFiles').doc(req.params.id).get()]);
    if (!item || !mapping.exists) return bad(res, req, 'NotFound', 404);
    const { StoredName: name, ContentType: contentType } = mapping.data();
    if (typeof name !== 'string' || name !== path.basename(name) || name.includes('\\') || name === '.' || name === '..') return bad(res, req, 'NotFound', 404);
    const file = path.resolve(uploadDir, name);
    try { await fs.access(file); } catch { return bad(res, req, 'NotFound', 404); }
    res.type(contentType || 'application/octet-stream');
    res.download(file, item.fileName, error => { if (error) next(error); });
  };
  return { getDocuments, getDocument, uploadDocument, downloadDocument };
}
module.exports = createDocumentsController;
module.exports.validFile = validFile;
