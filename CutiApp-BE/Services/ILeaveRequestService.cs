using CutiApp.DTOs;

namespace CutiApp.Services
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequestDetailResponse>> GetAllAsync();
        Task<List<LeaveRequestDetailResponse>> GetAllByUserIdAsync(long userId);
        Task<LeaveRequestDetailResponse?> GetByIdAsync(long id);
        Task<LeaveRequestDetailResponse> CreateAsync(CreateLeaveRequestRequests request, long userId);
        Task<LeaveRequestDetailResponse?> UpdateAsync(long id, UpdateLeaveRequestRequests request);
        //Task<bool> DeleteAsync(long id);
    }
}
