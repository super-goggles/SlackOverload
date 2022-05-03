using Microsoft.AspNetCore.Identity;
using SlackOverload.Data;

namespace SlackOverload.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public Question Question { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public List<AnswerComment>? Comments { get; set; }
        public int CorrectAnswer { get; set; } = 0;
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
    }
}
