using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CutiApp.DTOs
{
    public class UserResponse
    {
        public string Username { get; set; } = string.Empty;
        
        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
