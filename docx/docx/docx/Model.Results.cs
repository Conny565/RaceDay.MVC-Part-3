namespace RaceDay.MVC.Models
{
    public class Result
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string EventName { get; set; }
        public string FinishTime { get; set; } // e.g. 01:23:45
        public int Position { get; set; }
    }
}
