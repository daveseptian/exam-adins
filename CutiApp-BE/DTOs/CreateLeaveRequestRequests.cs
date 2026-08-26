using System.ComponentModel.DataAnnotations;

namespace CutiApp.DTOs
{
    public class CreateLeaveRequestRequests
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, MaxLength(500, ErrorMessage = "Alasan Cuti maksimal 500 karakter!")]
        public string Reason { get; set; } = string.Empty;
    }
}
