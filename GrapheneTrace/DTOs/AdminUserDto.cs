using GrapheneTrace.Models;

namespace GrapheneTrace.DTOs
{
    public class AdminUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
