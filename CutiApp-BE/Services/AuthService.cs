using CutiApp.DTOs;
using CutiApp.Models;
using CutiApp.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CutiApp.Services
{
    public class AuthService: IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IRepository<User> userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            List<User> users = await _userRepository.GetAllAsync();
            User? user = users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || user.Password != request.Password)
            {
                return null; // Controller akan return 401
            }

            Claim[] claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("userName", user.Username),
                new Claim("role", user.Role),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    double.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: creds);

            return new LoginResponse
            {
                Status = "success",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = new UserSummary
                {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = user.FullName,
                    Role = user.Role
                }
            };
        }
    }
}
