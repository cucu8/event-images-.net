using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using QrImageUploader.Data;
using resim_ekle.Entities;
using resim_ekle.DTO;
using System;

namespace resim_ekle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public UserController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(new { user.Id, user.Name, user.LocationName, user.Latitude, user.Longitude });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO.CreateUserDto createUserDto)
        {
            if (string.IsNullOrWhiteSpace(createUserDto.Name))
            {
                return BadRequest("Kullanıcı adı boş olamaz.");
            }

            var user = new User
            {
                Name = createUserDto.Name,
                Role = 0, // Default role
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new { user.Id, user.Name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // Hard delete - Kullanıcıyı veritabanından tamamen kaldır
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Kullanıcı veritabanından tamamen silindi.");
        }

    }
}
