using AcceptDocs.Application.Services;
using AcceptDocs.SharedKernel.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace AcceptDocs.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IValidator<AddDocumentDto> _addValidator;
        private readonly IValidator<UpdateDocumentDto> _updateValidator;

        public DocumentController(IDocumentService documentService, IValidator<AddDocumentDto> addValidator, IValidator<UpdateDocumentDto> updateValidator)
        {
            _documentService = documentService;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromForm] IFormFile file, [FromForm] string document)
        {
            var doc = JsonConvert.DeserializeObject<AddDocumentDto>(document);
            var result = _addValidator.Validate(doc);
            if (!result.IsValid) {
                return BadRequest(result);
            }

            if (file == null) {
                return BadRequest();
            }

            doc.FileName = Path.GetFileNameWithoutExtension(doc.FileName) + "_" + DateTime.Now + Path.GetExtension(doc.FileName);
            doc.FileName = doc.FileName.Replace(':', '-');

            var dir = Directory.GetCurrentDirectory();
            var saveDirectory = Path.Combine(dir, "wwwroot", "documents");
            Directory.CreateDirectory(saveDirectory);
            var filePath = Path.Combine(saveDirectory, doc.FileName);

            if(new [] { ".doc", ".docx" }.Contains(Path.GetExtension(doc.FileName.ToLowerInvariant()))) {
                var previewDirectory = Path.Combine(dir, "wwwroot", "previews");
                Directory.CreateDirectory(previewDirectory);
                var previewPath = Path.Combine(previewDirectory, Path.GetFileNameWithoutExtension(doc.FileName) + ".pdf");
                using var ms = new MemoryStream();
                file.CopyToAsync(ms);
                ms.Position = 0;

                var word = new Aspose.Words.Document(ms);
                using var outputStream = new MemoryStream();
                word.Save(previewPath, Aspose.Words.SaveFormat.Pdf);
            }

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write)) {
                file.CopyTo(stream);
            }

            var created = _documentService.Create(doc);

            return CreatedAtAction(nameof(Get), new { id = created.DocumentId }, created);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            if (id < 1)
                return BadRequest();
            if(_documentService.GetWithDetails(id) is DocumentDto document)
                return Ok(document);
            return NotFound();
        }

        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllForUserWithTypeAndFlow(int id)
        {
            return Ok(_documentService.GetAllForUserWithTypeAndFlow(id));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            if(_documentService.Get(id) is DocumentDto doc) {
                string file = doc.FileName;
                string docPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", file);
                if (System.IO.File.Exists(docPath))
                    System.IO.File.Delete(docPath);

                string previewPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "previews", Path.GetFileNameWithoutExtension(file) + ".pdf");
                if (System.IO.File.Exists(previewPath))
                    System.IO.File.Delete(previewPath);

                _documentService.Delete(id);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, [FromForm] IFormFile? file, [FromForm] string document)
        {
            var doc = JsonConvert.DeserializeObject<UpdateDocumentDto>(document);
            var result = _updateValidator.Validate(doc);
            if (!result.IsValid) {
                return BadRequest(result);
            }

            doc.FileName = Path.GetFileNameWithoutExtension(doc.FileName) + "_" + DateTime.Now + Path.GetExtension(doc.FileName);
            doc.FileName = doc.FileName.Replace(':', '-');

            if (file != null) {
                if (_documentService.Get(id) is DocumentDto old) {
                    string oldFileName = old.FileName;
                    string docPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", oldFileName);
                    if (System.IO.File.Exists(docPath))
                        System.IO.File.Delete(docPath);

                    string previewPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "previews", Path.GetFileNameWithoutExtension(oldFileName) + ".pdf");
                    if (System.IO.File.Exists(previewPath))
                        System.IO.File.Delete(previewPath);
                }

                var dir = Directory.GetCurrentDirectory();
                var saveDirectory = Path.Combine(dir, "wwwroot", "documents");
                Directory.CreateDirectory(saveDirectory);
                var filePath = Path.Combine(saveDirectory, doc.FileName);

                if (new[] { ".doc", ".docx" }.Contains(Path.GetExtension(doc.FileName.ToLowerInvariant()))) {
                    var previewDirectory = Path.Combine(dir, "wwwroot", "previews");
                    Directory.CreateDirectory(previewDirectory);
                    var previewPath = Path.Combine(previewDirectory, Path.GetFileNameWithoutExtension(doc.FileName) + ".pdf");
                    using var ms = new MemoryStream();
                    file.CopyToAsync(ms);
                    ms.Position = 0;

                    var word = new Aspose.Words.Document(ms);
                    using var outputStream = new MemoryStream();
                    word.Save(previewPath, Aspose.Words.SaveFormat.Pdf);
                }

                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write)) {
                    file.CopyTo(stream);
                }
            }

            _documentService.Update(doc);
            return NoContent();
        }

        [HttpGet("download/{fileName}")]
        public ActionResult DownloadFile(string fileName)
        {
            var dir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(dir, "wwwroot", "documents", fileName);
            if (!System.IO.File.Exists(filePath)) {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string mimeType)) {
                mimeType = "application/octet-stream";
            }

            return PhysicalFile(filePath, mimeType, fileName);
        }
    }
}
