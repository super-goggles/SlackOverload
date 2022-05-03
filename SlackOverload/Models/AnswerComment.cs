namespace SlackOverload.Models
{
    public class AnswerComment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public Answer Answer { get; set; } 
    }
}
