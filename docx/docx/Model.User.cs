namespace RaceDay.MVC.Models
{
    public class Users
    {
        public int userID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; } // organiser or participant
        public string profileImage { get; set; } // for wwwroot photo, e.g. /images/john.jpg
    }
}
