using AcceptDocs.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcceptDocs.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentFlowController : ControllerBase
    {
        private readonly IDocumentFlowService _documentFlowService;

        public DocumentFlowController(IDocumentFlowService documentFlowService)
        {
            _documentFlowService = documentFlowService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_documentFlowService.GetAll());
        }
    }
}
