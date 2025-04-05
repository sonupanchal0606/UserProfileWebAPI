using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using UserProfileWebAPI.Data;

namespace UserProfileWebAPI.Model
{
    public class User
    {
        public Guid UserId { get; set; } // Primary Key
        public string UserName { get; set; }
        public string Email { get; set; }

        // One-to-One: Navigation property
        public Profile Profile { get; set; }

        // One-to-Many: Navigation property
        public List<Post> Posts { get; set; } = new();

        // Many-to-Many: Navigation property
        public List<UserRole> UserRoles { get; set; } = new();
    }
}