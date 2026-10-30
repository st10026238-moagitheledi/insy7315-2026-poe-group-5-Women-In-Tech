using Google.Cloud.Firestore;
using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/documents")]
public sealed class DocumentsController(
    FirestoreDataStore store,
    FirestoreDb db,
    IWebHostEnvironment environment) : ControllerBase
{
    private string Folder => Path.Combine(environment.ContentRootPath, "App_Data", "uploads"); //(Microsoft Learn, 2026a).

    private static readonly Dictionary<string, string> Types =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = "application/pdf",
            [".docx"] =
                "application/vnd.openxmlformats-officedocument." +
                "wordprocessingml.document", //(OWASP Foundation, 2026).
            [".txt"] = "text/plain",
            [".png"] = "image/png",
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg"
        };

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await store.GetAllAsync<SchoolDocument>("schoolDocuments"));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await store.GetByIdAsync<SchoolDocument>( "schoolDocuments", id);

        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequestSizeLimit(12 * 1024 * 1024)] //(Microsoft Learn, 2026a).
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm] string? title)
    {
        var name = Path.GetFileName(file.FileName.Replace('\\', '/'));
        var extension = Path.GetExtension(name).ToLowerInvariant();

        if (file.Length <= 0 ||
            file.Length > 10 * 1024 * 1024 ||
            !Types.ContainsKey(extension) ||
            name.Length > 180 ||
            (title?.Length ?? 0) > 180)
        {
            return BadRequest(new
            {
                message =
                    "Choose a PDF, DOCX, TXT, PNG or JPG file up to 10 MB."
            });
        }

        Directory.CreateDirectory(Folder);

        var storedName = Guid.NewGuid().ToString("N") + extension;
        var path = Path.Combine(Folder, storedName);

        SchoolDocument? item = null;
        bool complete = false;

        try
        {
            await using (var output = new FileStream(path, FileMode.CreateNew))
            {
                await file.CopyToAsync( output, HttpContext.RequestAborted); //(Microsoft Learn, 2026b).
            }

            if (!ValidFile(path, extension))
            {
                return BadRequest(new
                {
                    message = "The file content does not match its extension."
                });
            }

            item = new SchoolDocument //(Google Cloud, 2026).
            {
                Title = string.IsNullOrWhiteSpace(title)
                    ? Path.GetFileNameWithoutExtension(name)
                    : title.Trim(),
                FileName = name,
                Category = "General",
                UploadedDate = DateTime.UtcNow
            };

            await store.AddAsync("schoolDocuments", item);

            item.FileUrl = $"/api/admin/documents/{item.Id}/file";

            var id = item.Id.ToString(System.Globalization.CultureInfo.InvariantCulture);

            await db.Collection("schoolDocumentFiles")
                .Document(id)
                .SetAsync(new
                {
                    StoredName = storedName,
                    ContentType = Types[extension]
                });

            await store.SaveAsync("schoolDocuments", item);

            complete = true;

            return CreatedAtAction( nameof(Get), new { id = item.Id }, item);
        }
        finally
        {
            if (!complete)
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                if (item is not null && item.Id > 0)
                {
                    var id = item.Id.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    await db.Collection("schoolDocuments")
                        .Document(id).DeleteAsync();

                    await db.Collection("schoolDocumentFiles")
                        .Document(id).DeleteAsync();
                }
            }
        }
    }

    [HttpGet("{id:int}/file")]
    public async Task<IActionResult> Download(int id)
    {
        var item = await store.GetByIdAsync<SchoolDocument>( "schoolDocuments", id); //(Google Cloud, 2026).

        var mapping = await db.Collection("schoolDocumentFiles")
            .Document(id.ToString(
                System.Globalization.CultureInfo.InvariantCulture))
            .GetSnapshotAsync();

        if (item is null || !mapping.Exists)
            return NotFound();

        var name = mapping.GetValue<string>("StoredName");

        if (name != Path.GetFileName(name))
            return NotFound();

        var path = Path.Combine(Folder, name);

        return System.IO.File.Exists(path)
            ? PhysicalFile(
                path,
                mapping.GetValue<string>("ContentType"),
                item.FileName)
            : NotFound();
    }

    private static bool ValidFile(string path, string extension)
    {
        var header = new byte[8];

        using (var input = System.IO.File.OpenRead(path))
        {
            input.ReadExactly( header.AsSpan(0, (int)Math.Min(8, input.Length)));
        }

        return extension switch
        {
            ".pdf" =>
                header[0] == '%' &&
                header[1] == 'P' &&
                header[2] == 'D' &&
                header[3] == 'F' &&
                header[4] == '-',

            ".png" => header.SequenceEqual( new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),

            ".jpg" or ".jpeg" =>
                header[0] == 255 &&
                header[1] == 216 &&
                header[2] == 255,

            ".docx" => IsWordDocument(path),
            ".txt" => IsText(path),
            _ => false
        };
    }

    private static bool IsWordDocument(string path)
    {
        try
        {
            using var zip = System.IO.Compression.ZipFile.OpenRead(path);

            return zip.GetEntry("word/document.xml") is not null;
        }
        catch (InvalidDataException)
        {
            return false;
        }
    }

    private static bool IsText(string path)
    {
        try
        {
            var bytes = System.IO.File.ReadAllBytes(path);

            _ = new System.Text.UTF8Encoding(false, true)
                .GetString(bytes);

            return !bytes.Contains((byte)0);
        }
        catch (System.Text.DecoderFallbackException)
        {
            return false;
        }
    }
}

/*Reference List
 
 Microsoft Corporation, "File Uploads in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
 Microsoft Corporation, "File Providers in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/file-providers
 OWASP Foundation, "Unrestricted File Upload Cheat Sheet," OWASP Cheat Sheet Series, 2026. [Online]. Available: https://cheatsheetseries.owasp.org/cheatsheets/Unrestricted_File_Upload_Cheat_Sheet.html
 Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest*/