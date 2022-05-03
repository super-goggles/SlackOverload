using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlackOverload.Models;

namespace SlackOverload.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Answer> Answer { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<AnswerComment> AnswerComment { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Vote> Vote { get; set; }
    }
}