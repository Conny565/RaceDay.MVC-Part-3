namespace RaceDay.MVC.Models
{
    public class events
    {
        public int eventID { get; set; }
        public int userID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string eventDate { get; set; }
        public string location { get; set; }
        public string eventTime { get; set; }
        public string distance { get; set; }

        public string eventType { get; set; }
        public string imagePath { get; set; } //  for wwwroot photo, e.g. /images/race1.jpg
    }
}
