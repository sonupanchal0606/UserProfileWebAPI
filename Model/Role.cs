namespace UserProfileWebAPI.Model
{
    public class Role
    {
        public Guid RoleId { get; set; } // Primary Key
        public string RoleName { get; set; }

        // Navigation property
        public List<UserRole> UserRoles { get; set; } = new();

    }
}
