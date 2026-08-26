using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CutiApp.Models
{
    [Table("LEAVE_BALANCE")]
    public class LeaveBalance
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [ForeignKey(nameof(User))]
        [Column("USER_ID")]
        public long UserId { get; set; }
        public User? User { get; set; }

        [Column("REMAINING_DAYS")]
        public int RemainingDays { get; set; } //integer karena jumlah hari cuti tahunan seharusnya tidak sampai lebih dari 32-bit
    }
}
