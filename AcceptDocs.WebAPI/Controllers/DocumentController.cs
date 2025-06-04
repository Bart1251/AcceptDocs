using AcceptDocs.Application.Services;
using AcceptDocs.SharedKernel.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AcceptDocs.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IValidator<AddDocumentDto> _validator;

        public DocumentController(IDocumentService documentService, IValidator<AddDocumentDto> validator)
        {
            _documentService = documentService;
            _validator = validator;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromForm] IFormFile file, [FromForm] string document)
        {
            var doc = JsonConvert.DeserializeObject<AddDocumentDto>(document);
            var result = _validator.Validate(doc);
            if (!result.IsValid) {
                return BadRequest(result);
            }

            if (file == null) {
                return BadRequest();
            }

            var dir = Directory.GetCurrentDirectory();
            var saveDirectory = Path.Combine(dir, "wwwroot", "documents");
            Directory.CreateDirectory(saveDirectory);
            var filePath = Path.Combine(saveDirectory, file.FileName);

            if(new [] { ".doc", ".docx" }.Contains(Path.GetExtension(file.FileName))) {
                var previewDirectory = Path.Combine(dir, "wwwroot", "previews");
                Directory.CreateDirectory(previewDirectory);
                var previewPath = Path.Combine(previewDirectory, Path.GetFileNameWithoutExtension(file.FileName) + ".pdf");
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

            int id = _documentService.Create(doc);
             
            return CreatedAtAction(nameof(Get), new { id });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            return Ok(_documentService.Get(id));
        }

        [HttpGet("all/{id}")]
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
    }
}
