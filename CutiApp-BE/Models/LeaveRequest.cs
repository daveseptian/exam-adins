using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CutiApp.Models
{
    [Table("LEAVE_REQUEST")]
    public class LeaveRequest
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [ForeignKey(nameof(User))]
        [Column("USER_ID")]
        public long UserId { get; set; }
        public User? User { get; set; }

        [Column("START_DATE")]
        public DateTime StartDate { get; set; }

        [Column("END_DATE")]
        public DateTime EndDate { get; set; }

        [Column("REASON")]
        public string Reason { get; set; } = string.Empty;

        [Column("STATUS")]
        public string Status { get; set; } = string.Empty;

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
