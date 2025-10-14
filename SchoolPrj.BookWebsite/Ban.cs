namespace SchoolPrj.BookWebsite
{
    public class Ban
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Username { get; set; }   
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string Role { get; set; }
        public string ProfileImagePath { get; set; }
    }
}