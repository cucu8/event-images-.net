using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrImageUploader.Data;
using resim_ekle.Entities;
using System;
using static resim_ekle.DTO.CreateUserDTO;

namespace resim_ekle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(new { user.Id, user.Name });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Kullanıcı adı boş olamaz.");
            }

            var user = new User
            {
                Name = dto.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { user.Id, user.Name });
        }
    }
}
