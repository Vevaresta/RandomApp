﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace RandomApp.SharedKernel.Authentication.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
