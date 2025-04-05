using UserProfileWebAPI.Data;
using UserProfileWebAPI.Model;

namespace UserProfileWebAPI
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Add initial data if it does not already exists
            if (!context.Users.Any())
            {
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "JohnDoe",
                    Email = "john@example.com",
                    Profile = new Profile
                    {
                        ProfileId = Guid.NewGuid(),
                        Bio = "Software Developer",
                        Website = "https://johndoe.dev"
                    },
                    Posts = new()
                    {
                        new Post
                        {
                            PostId = Guid.NewGuid(),
                            Title = "First Post",
                            Content = "Hello World!"
                        }
                    },
                    UserRoles = new()
                    {
                        new UserRole
                        {
                            Role = new Role
                            {
                                RoleId = Guid.NewGuid(),
                                RoleName = "Admin"
                            }
                        }
                    }
                };

                context.Users.Add(user);
                context.SaveChanges();
                //Console.WriteLine("Database seeded successfully.");
            }
            else
            {
                // Database has already been seeded
                return;
            }
        }
    }
}

/*public static void Initialize(ApplicationDbContext context)
{
    // Ensure the database is created
    context.Database.EnsureCreated();

    // Check if there are already roles in the database
    if (context.Roles.Any())
    {
        // Database has already been seeded
        return;
    }

    // Seed Roles
    var roles = new[]
    {
                new Role { RoleId = Guid.NewGuid(), RoleName = "Admin" },
                new Role { RoleId = Guid.NewGuid(), RoleName = "User" },
                new Role { RoleId = Guid.NewGuid(), RoleName = "Moderator" }
            };

    context.Roles.AddRange(roles);

    // Seed Users
    var users = new[]
    {
                new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "admin",
                    Email = "admin@example.com"
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "john_doe",
                    Email = "john@example.com"
                }
            };

    context.Users.AddRange(users);

    // Seed UserRoles
    var userRoles = new[]
    {
                new UserRole { UserId = users[0].UserId, RoleId = roles[0].RoleId }, // Admin user
                new UserRole { UserId = users[1].UserId, RoleId = roles[1].RoleId }  // Regular user
            };

    context.UserRoles.AddRange(userRoles);

    // Seed Posts
    var posts = new[]
    {
                new Post
                {
                    PostId = Guid.NewGuid(),
                    Title = "Welcome to our platform!",
                    Content = "This is your first post.",
                    UserId = users[1].UserId
                }
            };

    context.Posts.AddRange(posts);

    // Commit changes to the database
    context.SaveChanges();
}*/