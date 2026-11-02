'use strict';

const dictionaries = Object.fromEntries(['en-ZA', 'tn-ZA', 'nso-ZA'].map(c => [c, require(`../config/locales/${c}.json`)]));
function normalize(code) { return { en: 'en-ZA', 'en-za': 'en-ZA', tn: 'tn-ZA', 'tn-za': 'tn-ZA', nso: 'nso-ZA', 'nso-za': 'nso-ZA' }[String(code || '').trim().toLowerCase()] || null; }
function localization(req, res, next) {
  const requested = req.query['ui-culture'] || req.query.culture;
  const candidates = String(req.headers['accept-language'] || '').split(',').map((s, i) => { const [code, ...p] = s.trim().split(';'); return { code, q: p.length ? Number(p[0].replace('q=', '')) : 1, i }; }).filter(v => v.q > 0).sort((a, b) => b.q - a.q || a.i - b.i);
  req.culture = normalize(requested) || candidates.map(c => normalize(c.code)).find(Boolean) || 'en-ZA';
  req.t = key => dictionaries[req.culture][key] || dictionaries['en-ZA'][key] || key;
  res.set('Content-Language', req.culture); next();
}
module.exports = { normalize, localization, dictionaries };
