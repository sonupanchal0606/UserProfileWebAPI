namespace UserProfileWebAPI.Model
{
    public class Post
    {
        public Guid PostId { get; set; } // Primary Key
        public string Title { get; set; }
        public string Content { get; set; }

        // Foreign Key for One-to-Many
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
