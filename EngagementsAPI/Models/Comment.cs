using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngagementsAPI.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public int BlogId { get; set; }

        public string Body { get; set; }
    }
}
