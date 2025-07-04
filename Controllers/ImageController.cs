using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrImageUploader.Data;
using resim_ekle.Entities;
using System;

namespace resim_ekle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly DataContext _context;

        public ImageController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _context.Images
                .Select(img => new
                {
                    img.Id,
                    img.UserId,
                    ImageBase64 = Convert.ToBase64String(img.ImageData)
                })
                .ToListAsync();

            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound("Resim bulunamadı.");
            }

            var result = new
            {
                image.Id,
                image.UserId,
                ImageBase64 = Convert.ToBase64String(image.ImageData)
            };

            return Ok(result);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] Guid userId, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("Hiçbir dosya yüklenmedi.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var image = new Image
                {
                    ImageData = memoryStream.ToArray(),
                    UserId = userId
                };

                _context.Images.Add(image);
            }

            await _context.SaveChangesAsync();

            return Ok("Resimler başarıyla yüklendi.");
        }
    }
}
