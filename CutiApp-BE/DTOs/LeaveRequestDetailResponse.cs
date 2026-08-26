using System.ComponentModel.DataAnnotations;

namespace CutiApp.DTOs
{
    public class LeaveRequestDetailResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
