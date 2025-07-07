using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrImageUploader.Data;
using resim_ekle.Entities;

namespace resim_ekle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationImageController : ControllerBase
    {
        private readonly DataContext _context;

        public InvitationImageController(DataContext context)
        {
            _context = context;
        }


        // GET: api/InvitationImage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvitationImage>> GetInvitationImage(int id)
        {
            var invitationImage = await _context.InvitationImages
                .Include(ii => ii.User)
                .FirstOrDefaultAsync(ii => ii.Id == id);

            if (invitationImage == null)
            {
                return NotFound();
            }

            return invitationImage;
        }

        // GET: api/InvitationImage/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<InvitationImage>> GetInvitationImageByUserId(Guid userId)
        {
            var invitationImage = await _context.InvitationImages
                .Include(ii => ii.User)
                .FirstOrDefaultAsync(ii => ii.UserId == userId);

            if (invitationImage == null)
            {
                return NotFound();
            }

            return invitationImage;
        }

        // POST: api/InvitationImage
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<InvitationImage>> PostInvitationImage(IFormFile imageFile, Guid userId)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Image file is required");
            }

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(imageFile.ContentType.ToLower()))
            {
                return BadRequest("Only image files (JPEG, PNG, GIF) are allowed");
            }

            // Validate file size (max 5MB)
            if (imageFile.Length > 5 * 1024 * 1024)
            {
                return BadRequest("File size cannot exceed 5MB");
            }

            // Check if user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Check if user already has an invitation image
            var existingInvitation = await _context.InvitationImages
                .FirstOrDefaultAsync(ii => ii.UserId == userId);
            if (existingInvitation != null)
            {
                return BadRequest("User already has an invitation image");
            }

            // Convert file to byte array
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var invitationImage = new InvitationImage
            {
                ImageData = imageData,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.InvitationImages.Add(invitationImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvitationImage", new { id = invitationImage.Id }, invitationImage);
        }

        // PUT: api/InvitationImage/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutInvitationImage(int id, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Image file is required");
            }

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(imageFile.ContentType.ToLower()))
            {
                return BadRequest("Only image files (JPEG, PNG, GIF) are allowed");
            }

            // Validate file size (max 5MB)
            if (imageFile.Length > 5 * 1024 * 1024)
            {
                return BadRequest("File size cannot exceed 5MB");
            }

            var invitationImage = await _context.InvitationImages.FindAsync(id);
            if (invitationImage == null)
            {
                return NotFound();
            }

            // Convert file to byte array
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            invitationImage.ImageData = imageData;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvitationImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/InvitationImage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvitationImage(int id)
        {
            var invitationImage = await _context.InvitationImages.FindAsync(id);
            if (invitationImage == null)
            {
                return NotFound();
            }

            _context.InvitationImages.Remove(invitationImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvitationImageExists(int id)
        {
            return _context.InvitationImages.Any(e => e.Id == id);
        }
    }


}