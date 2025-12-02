using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Infrastructure.Persistence.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace miniBankingAPI.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly BankingDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(BankingDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> Login(string username, string password)
        {
            _logger.LogInformation("Login attempt for user: {Username}", username);
            
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
            
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for user: {Username}", username);
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            _logger.LogInformation("User {Username} logged in successfully", username);
            return GenerateJwtToken(user.Id, user.Username);
        }

        public async Task<int> Register(string username, string password, string email, int customerId)
        {
            _logger.LogInformation("Registration attempt for user: {Username}", username);
            
            if (await _context.Set<User>().AnyAsync(u => u.Username == username))
            {
                _logger.LogWarning("Registration failed - Username already exists: {Username}", username);
                throw new Exception("Username already exists");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email,
                CustomerId = customerId,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Username}, UserId: {UserId}", username, user.Id);
            return user.Id;
        }

        public string GenerateJwtToken(int userId, string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
