using BlogAPI.Data;
using EngagementsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EngagementsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;

        public CommentController(ApiDbContext apiDbContext)
        {
            _dbContext = apiDbContext;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Comment>> GetComments()
        {
            return _dbContext.Comments;
        }

        [HttpGet("{commentId:int}")]
        public async Task<ActionResult<Comment>> GetById(int commentId)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> Create(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Comment comment)
        {
            _dbContext.Comments.Update(comment);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{commentId:int}")]
        public async Task<ActionResult<Comment>> Delete(int commentId)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
