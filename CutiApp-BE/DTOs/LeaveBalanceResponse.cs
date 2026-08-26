using System.ComponentModel.DataAnnotations;

namespace CutiApp.DTOs
{
    public class LeaveBalanceResponse
    {
        public long UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int RemainingDays { get; set; }
    }
}
