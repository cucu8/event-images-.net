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

        // GET: api/Image/download/{id}
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound("Resim bulunamadı.");
            }

            // Dosya tipini belirle (varsayılan olarak JPEG)
            var contentType = "image/jpeg";
            var fileName = $"image_{id}.jpg";

            return File(image.ImageData, contentType, fileName);
        }

        // GET: api/Image/user/{userId}/download
        [HttpGet("user/{userId}/download")]
        public async Task<IActionResult> DownloadUserImages(Guid userId)
        {
            var images = await _context.Images
                .Where(img => img.UserId == userId)
                .ToListAsync();

            if (images == null || !images.Any())
            {
                return NotFound("Bu kullanıcıya ait resim bulunamadı.");
            }

            // Tek resim varsa direkt döndür
            if (images.Count == 1)
            {
                var singleImage = images.First();
                var contentType = "image/jpeg";
                var fileName = $"image_{singleImage.Id}.jpg";
                return File(singleImage.ImageData, contentType, fileName);
            }

            // Birden fazla resim varsa ZIP olarak döndür
            using var memoryStream = new MemoryStream();
            using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var image in images)
                {
                    var entry = archive.CreateEntry($"image_{image.Id}.jpg");
                    using var entryStream = entry.Open();
                    await entryStream.WriteAsync(image.ImageData, 0, image.ImageData.Length);
                }
            }

            memoryStream.Position = 0;
            return File(memoryStream.ToArray(), "application/zip", $"user_{userId}_images.zip");
        }
    }
}
