namespace GrapheneTrace.Models
{
    public enum UserRole
    {
        Admin = 0,
        Employee = 1
    }

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
