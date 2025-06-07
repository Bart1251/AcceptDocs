using AcceptDocs.Application.Services;
using AcceptDocs.SharedKernel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcceptDocs.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AcceptanceRequestController : ControllerBase
    {
        private readonly IAcceptanceRequestService _acceptanceRequestService;

        public AcceptanceRequestController(IAcceptanceRequestService acceptanceRequestService)
        {
            _acceptanceRequestService = acceptanceRequestService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            return Ok(_acceptanceRequestService.Get(id));
        }

        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllForUser(int id)
        {
            return Ok(_acceptanceRequestService.GetAllForUser(id));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GiveFeedback(int id, AddDocumentFeedbackDto dto)
        {
            _acceptanceRequestService.GiveFeedback(dto);
            return NoContent();
        }
    }
}
