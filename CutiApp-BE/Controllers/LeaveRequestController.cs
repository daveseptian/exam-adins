using CutiApp.DTOs;
using CutiApp.Exceptions;
using CutiApp.Models;
using CutiApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CutiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController: ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var leaveRequests = await _leaveRequestService.GetAllAsync();
            var response = leaveRequests;

            return Ok(response);
        }

        [HttpGet("User")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetLeaveRequestUser()
        {
            long userId = long.Parse(User.FindFirst("userId")!.Value);

            var leaveRequestsUser = await _leaveRequestService.GetAllByUserIdAsync(userId);
            if (leaveRequestsUser == null) return NotFound();
            return Ok(leaveRequestsUser);
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> GetLeaveRequest(long id)
        {
            var leaveRequest = await _leaveRequestService.GetByIdAsync(id);
            if (leaveRequest == null) return NotFound();
            return Ok(leaveRequest);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestRequests request)
        {
            long userId = long.Parse(User.FindFirst("userId")!.Value);

            try
            {
                LeaveRequestDetailResponse? result = await _leaveRequestService.CreateAsync(request, userId);
                return Created($"/api/bookcopies/{result!.Id}", result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { status = "error", message = ex.Message });
            }
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateLeaveRequest(long id, [FromBody] UpdateLeaveRequestRequests request)
        {
            long userId = long.Parse(User.FindFirst("userId")!.Value);
            try
            {
                LeaveRequestDetailResponse? result = await _leaveRequestService.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { status = "error", message = ex.Message });
            }
        }
    }
}
