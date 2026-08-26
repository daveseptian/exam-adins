using System.ComponentModel.DataAnnotations;

namespace CutiApp.DTOs
{
    public class UpdateLeaveRequestRequests
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";
    }
}
