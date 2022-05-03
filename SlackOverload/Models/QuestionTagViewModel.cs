using Microsoft.AspNetCore.Mvc.Rendering;

namespace SlackOverload.Models
{
    public class QuestionTagViewModel
    {
        public Question Question { get; set; }
        public Tag Tags { get; set; }

        public QuestionTagViewModel()
        {
            
        }
    }
}
