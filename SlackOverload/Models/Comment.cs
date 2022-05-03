namespace SlackOverload.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public Question Question { get; set; }
    }
}
