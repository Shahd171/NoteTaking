using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Note_Taking_App.Applcation.Services;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.ResponseModel;
using Note_Taking_App.Core.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
namespace Note_Taking_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {
            _authService = authService;


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userDto)
        {
            try
            {
                var data = await _authService.Login(userDto);
                var response = ApiResponse<object>.SuccessResponse(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<object>.ErrorResponse(new List<string> { ex.Message });
                return Unauthorized(response);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {

            try
            {
                var data = await _authService.Register(userDto);
                var response = ApiResponse<object>.SuccessResponse(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<object>.ErrorResponse(new List<string> { ex.Message });
                return BadRequest(response);
            }
        }
    }
}
