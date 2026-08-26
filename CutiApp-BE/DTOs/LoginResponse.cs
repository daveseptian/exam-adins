namespace CutiApp.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Status { get; set; } = "Success";
        public UserSummary User { get; set; } = new();
    }

    public class UserSummary
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
