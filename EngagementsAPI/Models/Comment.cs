using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngagementsAPI.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int BlogId { get; set; }

        public string Body { get; set; }

        public string CreatedBy { get; set; }
    }
}
