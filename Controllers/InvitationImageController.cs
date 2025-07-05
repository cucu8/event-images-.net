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
        public async Task<ActionResult<InvitationImage>> PostInvitationImage([FromBody] CreateInvitationImageDTO createDto)
        {
            // Check if user exists
            var user = await _context.Users.FindAsync(createDto.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Check if user already has an invitation image
            var existingInvitation = await _context.InvitationImages
                .FirstOrDefaultAsync(ii => ii.UserId == createDto.UserId);
            if (existingInvitation != null)
            {
                return BadRequest("User already has an invitation image");
            }

            var invitationImage = new InvitationImage
            {
                ImageData = createDto.ImageData,
                UserId = createDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.InvitationImages.Add(invitationImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvitationImage", new { id = invitationImage.Id }, invitationImage);
        }

        // PUT: api/InvitationImage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvitationImage(int id, [FromBody] CreateInvitationImageDTO updateDto)
        {
            var invitationImage = await _context.InvitationImages.FindAsync(id);
            if (invitationImage == null)
            {
                return NotFound();
            }

            invitationImage.ImageData = updateDto.ImageData;
            
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

    public class CreateInvitationImageDTO
    {
        public byte[] ImageData { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}