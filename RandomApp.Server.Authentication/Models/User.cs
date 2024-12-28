using Microsoft.AspNetCore.Identity;

namespace RandomApp.Server.Authentication.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
