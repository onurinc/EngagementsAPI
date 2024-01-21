using EngagementsAPI.Data;
using EngagementsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngagementsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        
        public CommentsController(ApiDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Comments.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Comment>> GetById(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null) return NotFound();
                
            return Ok(comment);
        }

        [HttpPost("AddComment")]
        public async Task<ActionResult<Comment>> Create(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdateComment")]
        public async Task<IActionResult> Update(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{commentId:int}")]
        public async Task<ActionResult<Comment>> Delete(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetAllByUsername/{username}")]
        public async Task<IActionResult> GetAllByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username must be provided.");
            }

            var commentsByCreatedBy = await _context.Comments
                .Where(c => c.CreatedBy == username)
                .ToListAsync();

            if (commentsByCreatedBy.Any())
            {
                return Ok(commentsByCreatedBy);
            }
            
            return NotFound($"No comments found for user with username {username}");
        }


        [HttpDelete("DeleteAllByUsername/{username}")]
        public async Task<IActionResult> DeleteAllByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username must be provided.");
            }

            var commentsToDelete = await _context.Comments
                .Where(c => c.CreatedBy == username)
                .ToListAsync();

            if (commentsToDelete.Any())
            {
                _context.Comments.RemoveRange(commentsToDelete);
                await _context.SaveChangesAsync();
                return Ok($"Successfully deleted all comments for user with username {username}");
            }

            return NotFound($"No comments found for user with username {username}");
        }

    }
}
