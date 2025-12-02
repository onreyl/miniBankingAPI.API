namespace miniBankingAPI.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        
        // Navigation Property
        public Customer Customer { get; set; } = null!;
    }
}
