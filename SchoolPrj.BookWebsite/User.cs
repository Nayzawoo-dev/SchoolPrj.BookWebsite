namespace SchoolPrj.BookWebsite
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImagePath { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string EmailVerificationCode { get; set; }
        public int BanStatus { get; set; } = 0;
    }
}
