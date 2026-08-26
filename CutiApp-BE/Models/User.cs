using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CutiApp.Models
{
    [Table("USER")]
    public class User
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("USERNAME")]//Unique
        public string Username { get; set; } = string.Empty;

        [Column("PASSWORD")]
        public string Password { get; set; } = string.Empty;

        [Column("FULLNAME")]
        public string FullName { get; set; } = string.Empty;

        [Column("ROLE")]
        public string Role { get; set; } = string.Empty;

        public LeaveBalance LeaveBalance { get; set; } = new LeaveBalance();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
