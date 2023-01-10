using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopsApi.Models;
using ShopsApi.Services;

namespace ShopsApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public ActionResult Register([FromForm] RegisterUserDto dto)
        {
            _service.RegisterUser(dto);
            return Ok("Account created");
        }

        [HttpPost("login")]
        public ActionResult Login([FromForm] LoginDto dto)
        {
            string token = _service.GenerateJwt(dto);
            return Ok($"Welcome. Please write down your token : {token}");
        }
        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete([FromForm] int userId)
        {
            _service.DeleteUser(userId);
            return Ok("User deleted");
                
        }

    }
}
