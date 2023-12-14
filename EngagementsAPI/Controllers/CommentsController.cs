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

    }
}
