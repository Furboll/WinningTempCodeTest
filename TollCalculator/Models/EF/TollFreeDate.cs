namespace TollCalculator.Models.EF
{
    public class TollFreeDate
    {
        public int Id { get; set; } // GUID om det behövs istället       
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime FullDate { get; set; }

        public TollFreeDate(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
            FullDate = new DateTime(Year, Month, Day);
        }
    }
}
