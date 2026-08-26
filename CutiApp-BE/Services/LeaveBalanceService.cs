using CutiApp.DTOs;
using CutiApp.Exceptions;
using CutiApp.Models;
using CutiApp.Repositories;

namespace CutiApp.Services
{
    public class LeaveBalanceService: ILeaveBalanceService
    {
        private readonly IRepository<LeaveBalance> _leaveBalanceRepository;
        private readonly ILogger<LeaveBalanceService> _logger;

        public LeaveBalanceService(IRepository<LeaveBalance> leaveBalanceRepository, ILogger<LeaveBalanceService> logger)
        {
            _leaveBalanceRepository = leaveBalanceRepository;
            _logger = logger;
        }
        public async Task<List<LeaveBalanceResponse>> GetAllAsync()
        {
            var leaveBalances = await _leaveBalanceRepository.GetAllAsync("User");

            var response = leaveBalances.Select(leaveBalance => new LeaveBalanceResponse
            {
                UserId = leaveBalance.UserId,
                RemainingDays = leaveBalance.RemainingDays,
                Username = leaveBalance.User!.Username,
                Role = leaveBalance.User!.Role
            }).ToList();

            _logger.LogInformation("Got all Leave Balance Data");

            return response;
        }
        public async Task<LeaveBalanceResponse?> GetByIdAsync(long id)
        {
            try
            {
                var leaveBalanceById = await _leaveBalanceRepository.GetByIdAsync(id, "User");

                if (leaveBalanceById == null)
                {
                    _logger.LogWarning($"Leave Balance with ID {id} does not exists!");
                    return null;
                }

                _logger.LogInformation($"Leave Balance for {leaveBalanceById.User!.Username} has been found!");
                return ToDetailResponse(leaveBalanceById);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        public async Task<LeaveBalanceResponse?> UpdateAsync(long id, LeaveBalanceRequest request)
        {
            await _leaveBalanceRepository.BeginTransactionAsync();

            try
            {
                var leaveBalance = await _leaveBalanceRepository.GetByIdAsync(id, "User");
                if( leaveBalance == null )
                {
                    throw new NotFoundException($"Leave Balance with with ID ${id} not found!");
                }

                _logger.LogInformation($"Updating Leave Balance for {leaveBalance.User!.Username}...");

                leaveBalance.RemainingDays = request.RemainingDays;

                await _leaveBalanceRepository.UpdateAsync(leaveBalance);

                await _leaveBalanceRepository.SaveChangesAsync();
                await _leaveBalanceRepository.CommitTransactionAsync();

                var updated = await _leaveBalanceRepository.GetByIdAsync(id, "User");

                _logger.LogInformation($"Leave Balance for {updated!.User!.Username} have been updated to {updated.RemainingDays} days!");

                return ToDetailResponse(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Leave Balance for User ID : {request.UserId} has failed to be updated");
                await _leaveBalanceRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public LeaveBalanceResponse ToDetailResponse(LeaveBalance leaveBalance)
        {
            return new LeaveBalanceResponse
            {
                UserId = leaveBalance.UserId,
                RemainingDays = leaveBalance.RemainingDays,
                Username = leaveBalance.User!.Username,
                Role = leaveBalance.User!.Role
            };
        }
    }
}
