namespace miniBankingAPI.Application.DTOs
{
    public record LoginRequest(string Username, string Password);
    public record RegisterRequest(string Username, string Password, string Email, int CustomerId);

    public record LoginResponse(string Token);
    public record RegisterResponse(int UserId);
}
