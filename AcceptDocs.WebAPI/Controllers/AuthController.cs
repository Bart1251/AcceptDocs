using AcceptDocs.Application.Services;
using AcceptDocs.SharedKernel.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AcceptDocs.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login(UserLoginDto dto)
        {
            var token = _userService.Login(dto);
            return Ok(new { token = token });
        }
    }
}
