using CutiApp.DTOs;
using CutiApp.Exceptions;
using CutiApp.Models;
using CutiApp.Repositories;

namespace CutiApp.Services
{
    public class LeaveRequestService: ILeaveRequestService
    {
        private readonly IRepository<LeaveRequest> _leaveRequestRepository;
        private readonly ILogger<LeaveRequestService> _logger;

        public LeaveRequestService(IRepository<LeaveRequest> leaveRequestRepository, ILogger<LeaveRequestService> logger)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _logger = logger;
        }

        public async Task<List<LeaveRequestDetailResponse>> GetAllAsync()
        {
            var leaveRequests = await _leaveRequestRepository.GetAllAsync("User");

            var response = leaveRequests.Select(leaveRequest => new LeaveRequestDetailResponse
            {
                Id = leaveRequest.Id,
                UserId = leaveRequest.UserId,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                Username = leaveRequest.User!.Username,
                Role = leaveRequest.User!.Role,
                Status = leaveRequest.Status,
                FullName = leaveRequest.User!.FullName
            }).ToList();

            _logger.LogInformation("Got all Leave Request Data");

            return response;
        }

        public async Task<List<LeaveRequestDetailResponse>> GetAllByUserIdAsync(long userId)
        {
            var leaveRequests = await _leaveRequestRepository.GetAllAsync("User");

            var response = leaveRequests.Where(lr => lr.UserId == userId).Select(leaveRequest => new LeaveRequestDetailResponse
            {
                Id = leaveRequest.Id,
                UserId = userId,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                Username = leaveRequest.User!.Username,
                Role = leaveRequest.User!.Role,
                Status = leaveRequest.Status,
                FullName = leaveRequest.User!.FullName
            }).ToList();

            _logger.LogInformation("Got all Leave Request Data");

            return response;
        }
        public async Task<LeaveRequestDetailResponse?> GetByIdAsync(long id)
        {
            try
            {
                var leaveRequestById = await _leaveRequestRepository.GetByIdAsync(id, "User");

                if (leaveRequestById == null)
                {
                    _logger.LogWarning($"Leave Request with ID {id} does not exists!");
                    return null;
                }

                _logger.LogInformation($"Leave Balance for {leaveRequestById.User!.Username} has been found!");
                return ToDetailResponse(leaveRequestById);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LeaveRequestDetailResponse> CreateAsync(CreateLeaveRequestRequests request, long userId)
        {
            await _leaveRequestRepository.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Creating Leave Request for User ID : {userId}...", request.UserId);

                var utcDateTime = DateTime.Now.ToUniversalTime();

                var leaveRequest = new LeaveRequest
                {
                    UserId = userId,
                    Status = "Pending",
                    StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
                    EndDate = request.EndDate.ToUniversalTime().Date,
                    Reason = request.Reason,
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };

                int hariCuti = CountDays(leaveRequest.StartDate, leaveRequest.EndDate);

                await _leaveRequestRepository.AddAsync(leaveRequest);

                await _leaveRequestRepository.SaveChangesAsync();
                await _leaveRequestRepository.CommitTransactionAsync();

                var created = await _leaveRequestRepository.GetByIdAsync(leaveRequest.Id, "User");

                _logger.LogInformation($"Leave Request from {created!.User!.Username} for {hariCuti} days has been successfully created!");
                return ToDetailResponse(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Leave Request for User ID : {request.UserId} has failed!");
                await _leaveRequestRepository.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<LeaveRequestDetailResponse?> UpdateAsync(long id, UpdateLeaveRequestRequests request)
        {
            await _leaveRequestRepository.BeginTransactionAsync();
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id, "User", "User.LeaveBalance");
                if (leaveRequest == null)
                {
                    throw new NotFoundException($"Leave Request with with ID ${id} not found!");
                }

                _logger.LogInformation($"Updating Leave Request for {leaveRequest.User!.Username}...");

                if(leaveRequest.User!.Role != "Manager")
                {
                    throw new Exception("Anda tidak bisa mengubah Leave Request!");
                }

                if(IsPending(request.Status) && IsNotDone(leaveRequest.Status))
                {
                    throw new Exception("Status tidak valid!");
                }

                if(!CheckDayOffQuota(leaveRequest.User!.LeaveBalance.RemainingDays, leaveRequest.StartDate, leaveRequest.EndDate))
                {
                    throw new Exception("Anda tidak memiliki hari cuti yang cukup!");
                }

                leaveRequest.Status = request.Status;
                if(string.Equals(leaveRequest.Status, "Approved"))
                {
                    leaveRequest.User!.LeaveBalance.RemainingDays -= CountDays(leaveRequest.StartDate, leaveRequest.EndDate);
                }

                await _leaveRequestRepository.UpdateAsync(leaveRequest);

                await _leaveRequestRepository.SaveChangesAsync();
                await _leaveRequestRepository.CommitTransactionAsync();

                var updated = await _leaveRequestRepository.GetByIdAsync(id, "User");

                _logger.LogInformation($"Leave Request for {updated!.User!.Username} have been {updated.Status}!");

                return ToDetailResponse(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Leave Request for User ID : {request.UserId} has failed to be updated");
                await _leaveRequestRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<LeaveRequestDetailResponse?> DeleteAsync(long id)
        {
            await _leaveRequestRepository.BeginTransactionAsync();

            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id, "User");
                if (leaveRequest == null) throw new NotFoundException($"Leave Request with ID : ${id} not found!");

                _logger.LogInformation($"Deleting Leave Request from {leaveRequest.User!.Username} for {leaveRequest.StartDate} - {leaveRequest.EndDate}!");

                await _leaveRequestRepository.DeleteAsync(leaveRequest);

                await _leaveRequestRepository.SaveChangesAsync();
                await _leaveRequestRepository.CommitTransactionAsync();

                _logger.LogInformation($"Leave Request from {leaveRequest.User!.Username} for {leaveRequest.StartDate} - {leaveRequest.EndDate} has been cancelled!");

                var deleted = await _leaveRequestRepository.GetByIdAsync(leaveRequest.Id, "User");

                return ToDetailResponse(deleted);

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to delete Leave Request with ID : {id}");
                await _leaveRequestRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public LeaveRequestDetailResponse ToDetailResponse(LeaveRequest leaveRequest)
        {
            return new LeaveRequestDetailResponse
            {
                Id = leaveRequest.Id,
                UserId = leaveRequest.UserId,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                Username = leaveRequest.User!.Username,
                Role = leaveRequest.User!.Role
            };
        }

        public int CountDays(DateTime startDate, DateTime endDate)
        {
            TimeSpan difference = endDate - startDate;
            return difference.Days + 1;
        }

        public bool IsPending(string status)
        {
            return !string.Equals(status, "Approved") && !string.Equals(status, "Rejected");
        }

        public bool IsNotDone(string status)
        {
            return !string.Equals(status, "Approved") && !string.Equals(status, "Rejected");
        }

        public bool CheckDayOffQuota(int remainingDays, DateTime startDate, DateTime endDate)
        {
            int difference = CountDays(startDate, endDate);
            return remainingDays >= difference ? true : false;
        }
    }
}
