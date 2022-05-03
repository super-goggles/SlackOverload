using SlackOverload.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlackOverload.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public  List<Answer>? Answers { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Tag>? Tags { get; set; }
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
    }
}
