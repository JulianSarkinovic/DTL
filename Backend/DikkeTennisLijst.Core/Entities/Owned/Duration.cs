namespace DikkeTennisLijst.Core.Entities.Owned
{
    public class Duration
    {
        private Duration()
        {
        }

        public Duration(DateTimeOffset start, DateTimeOffset end)
        {
            if (start > end) throw new Exception("Start date is greater than end date.");
            Start = start;
            End = end;
        }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public int DurationInMinutes => (End - Start).Minutes;
    }
}