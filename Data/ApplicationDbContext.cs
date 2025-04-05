using Microsoft.EntityFrameworkCore;
using UserProfileWebAPI.Model;

namespace UserProfileWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the Many-to-Many relationship
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // to seed Initial Data without calling DbInitializer Class
            // Data is seeded only when there is no data in the Database
            modelBuilder.Entity<Role>().HasData(
                new Role() { RoleId = Guid.NewGuid(), RoleName = "Admin" },
                new Role() { RoleId = Guid.NewGuid(), RoleName = "User" },
                new Role() { RoleId = Guid.NewGuid(), RoleName = "Moderator" }
                );
        }
    }
}
