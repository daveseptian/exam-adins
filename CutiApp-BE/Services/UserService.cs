using CutiApp.DTOs;
using CutiApp.Models;
using CutiApp.Repositories;

namespace CutiApp.Services
{
    public class UserService: IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IRepository<User> userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var validUsers = users.Where(u => u.Role == "Manager" || u.Role == "Employee");

            var response = validUsers.Select(user => new UserResponse
            {
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            }).ToList();

            _logger.LogInformation("Got all User Data");

            return response;
        }

        public async Task<UserResponse?> GetByIdAsync(long id)
        {
            try
            {
                var userById = await _userRepository.GetByIdAsync(id);

                if(userById == null)
                {
                    _logger.LogWarning("User with ID {userId} does not exists!", id);
                    return null;
                }

                _logger.LogInformation("User {userName} has been found!", userById.Username);
                return ToDetailResponse(userById);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserResponse ToDetailResponse(User user)
        {
            return new UserResponse
            {
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            };
        }
    }
}
