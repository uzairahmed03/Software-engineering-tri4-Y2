using GrapheneTrace.DTOs;
using GrapheneTrace.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _service;

        public AdminController(AdminService service)
        {
            _service = service;
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(AdminUserDto dto)
        {
            var user = await _service.CreateUser(dto);
            return Ok(user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _service.GetUsers());
        }
    }
}
