using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrImageUploader.Data;
using resim_ekle.Entities;

namespace resim_ekle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly DataContext _context;

        public VideoController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Video
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
        {
            return await _context.Videos
                .Include(v => v.User)
                .ToListAsync();
        }

        // GET: api/Video/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {
            var video = await _context.Videos
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }

        // GET: api/Video/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideosByUserId(Guid userId)
        {
            var videos = await _context.Videos
                .Include(v => v.User)
                .Where(v => v.UserId == userId)
                .ToListAsync();

            return videos;
        }

        // POST: api/Video
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Video>> PostVideo(IFormFile videoFile, Guid userId)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                return BadRequest("Video file is required");
            }

            // Validate file type
            var allowedTypes = new[] { "video/mp4", "video/avi", "video/mov", "video/wmv", "video/webm" };
            if (!allowedTypes.Contains(videoFile.ContentType.ToLower()))
            {
                return BadRequest("Only video files (MP4, AVI, MOV, WMV, WebM) are allowed");
            }

            // Validate file size (max 50MB)
            if (videoFile.Length > 50 * 1024 * 1024)
            {
                return BadRequest("File size cannot exceed 50MB");
            }

            // Check if user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Convert file to byte array
            byte[] videoData;
            using (var memoryStream = new MemoryStream())
            {
                await videoFile.CopyToAsync(memoryStream);
                videoData = memoryStream.ToArray();
            }

            var video = new Video
            {
                VideoData = videoData,
                FileName = videoFile.FileName,
                ContentType = videoFile.ContentType,
                FileSize = videoFile.Length,
                DurationSeconds = 0, // Bu değer video işleme kütüphanesi ile hesaplanabilir
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideo", new { id = video.Id }, video);
        }

        // DELETE: api/Video/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Video/download/5
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            return File(video.VideoData, video.ContentType, video.FileName);
        }

    }
}