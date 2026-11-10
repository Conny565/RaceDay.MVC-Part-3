namespace RaceDay.MVC.Models
{
    public class Enrolment
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string Location { get; set; }
    }
}
