using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrImageUploader.Data;
using resim_ekle.DTO;
using resim_ekle.Entities;

namespace resim_ekle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {

        private readonly DataContext _context;

        public CommentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseCommentDTO>>> GetComments()
        {
            var comments = await _context.Comments
                                         .Include(c => c.User)
                                         .ToListAsync();

            if (comments == null || !comments.Any())
            {
                return NotFound("Hiç yorum bulunamadı.");
            }

            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseCommentDTO>> GetComment(int id)
        {
            var comment = await _context.Comments
                                        .Include(c => c.User)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound($"ID'si {id} olan yorum bulunamadı.");
            }

            var commentDto = new ResponseCommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                UserId = comment.UserId,
                UserName = comment.User?.Name
            };

            return Ok(commentDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseCommentDTO>> PostComment(CreateCommentDTO createCommentDto)
        {
         
            var user = await _context.Users.FindAsync(createCommentDto.UserId);
            if (user == null)
            {
                return BadRequest($"ID'si {createCommentDto.UserId} olan kullanıcı bulunamadı.");
            }

            
            var comment = new Comment
            {
                Content = createCommentDto.Content, 
                UserId = createCommentDto.UserId   
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            
            var commentDto = new ResponseCommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                UserId = comment.UserId,
                UserName = user.Name
            };

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentDto);
        }
    }
}
