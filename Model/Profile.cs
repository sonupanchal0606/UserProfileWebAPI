namespace UserProfileWebAPI.Model
{
    public class Profile
    {
        public Guid ProfileId { get; set; } // Primary Key
        public string Bio { get; set; }
        public string Website { get; set; }

        // Foreign Key for One-to-One Relationship
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
