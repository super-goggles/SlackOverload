namespace SlackOverload.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Question>? Questions { get; set; } 
    }
}
