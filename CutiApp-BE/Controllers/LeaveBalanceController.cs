using CutiApp.DTOs;
using CutiApp.Exceptions;
using CutiApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CutiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveBalanceController: ControllerBase
    {
        private readonly ILeaveBalanceService _leaveBalanceService;

        public LeaveBalanceController(ILeaveBalanceService leaveBalanceService)
        {
            _leaveBalanceService = leaveBalanceService;
        }

        [HttpGet]
        [Authorize
            (Roles = "Manager")
            ]
        public async Task<IActionResult> GetLeaveBalances()
        {
            var leaveBalances = await _leaveBalanceService.GetAllAsync();
            var response = leaveBalances;

            return Ok(response);
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetLeaveBalance(long id)
        {
            var leaveBalance = await _leaveBalanceService.GetByIdAsync(id);
            if (leaveBalance == null) return NotFound();
            return Ok(leaveBalance);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateLeaveBalance(long id, [FromBody] LeaveBalanceRequest request)
        {
            long userId = long.Parse(User.FindFirst("userId")!.Value);
            try
            {
                LeaveBalanceResponse? result = await _leaveBalanceService.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { status = "error", message = ex.Message });
            }
        }
    }
}
