namespace UserProfileWebAPI.Model
{
    public class UserRole
    {
        // Bridging table for Many-to-Many Relationship
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }

}

