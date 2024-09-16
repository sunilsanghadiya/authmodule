using authmodule.Models.DTOs;
using authmodule.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace authmodule.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers() 
        {
            Result? result = await _userService.GetUsers();
            return Ok(result.ApiResult);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto) 
        {
            Result? result = await _userService.Login(loginDto);
            return Ok(result.ApiResult);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto) 
        {
            Result? result = await _userService.Register(registerDto);
            return Ok(result.ApiResult);
        }
    }
}