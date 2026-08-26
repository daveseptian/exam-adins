using CutiApp.DTOs;

namespace CutiApp.Services
{
    public interface ILeaveBalanceService
    {
        Task<List<LeaveBalanceResponse>> GetAllAsync();
        Task<LeaveBalanceResponse?> GetByIdAsync(long id);
        //Task<LeaveBalanceResponse> CreateAsync(LeaveBalanceRequest request);
        Task<LeaveBalanceResponse?> UpdateAsync(long id, LeaveBalanceRequest request);
        //Task<bool> DeleteAsync(long id);
    }
}
