namespace miniBankingAPI.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(string username, string password);
        Task<int> Register(string username, string password, string email, int customerId);
        string GenerateJwtToken(int userId, string username);
    }
}
