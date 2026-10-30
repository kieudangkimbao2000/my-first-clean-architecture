using Application.Interface;
using Application.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.APIController
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginRequest)
        {
            var response = _authService.Login(loginRequest);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            CookieOptions option = new CookieOptions();
            
            option.HttpOnly = true;
            option.Secure = true;
            option.SameSite = SameSiteMode.Strict;

            Response.Cookies.Append("auth_token", response.Data.Token, option);
            response.Data.Token = null;

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO req)
        {
            var response = _authService.Register(req);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}