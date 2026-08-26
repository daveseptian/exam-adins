using System.ComponentModel.DataAnnotations;

namespace CutiApp.DTOs
{
    public class LeaveBalanceRequest
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public int RemainingDays { get; set; }
    }
}
