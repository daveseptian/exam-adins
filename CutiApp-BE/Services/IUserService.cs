using CutiApp.DTOs;

namespace CutiApp.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(long id);
        //Task<UserResponse> CreateAsync(UserRequest request);
        //Task<UserResponse?> UpdateAsync(long id, UserRequest request);
        //Task<bool> DeleteAsync(long id);
    }
}
