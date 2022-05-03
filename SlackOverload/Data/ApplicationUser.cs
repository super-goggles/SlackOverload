using Microsoft.AspNetCore.Identity;

namespace SlackOverload.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int Reputation { get; set; } = 0;
    }
}
