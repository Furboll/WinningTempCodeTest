using TollCalculator.Data;
using TollCalculator.Models.EF;
using TollCalculator.Models.Enums;
using TollCalculator.Service.Interface;

namespace TollCalculator.Service
{
    public class TollCalculatorService : ITollCalculatorService
    {
        private readonly TollDbContext _context;
        public TollCalculatorService(TollDbContext context)
        {
            _context = context;
        }
        public int GetTollFee(VehicleType vehicle, DateTime[] dates)
        {
            var totalFee = 0;
            DateTime lastPassage = DateTime.MinValue;

            foreach (DateTime date in dates)
            {
                var nextFee = CalculateTollFee(date, vehicle);
                var tempFee = lastPassage != DateTime.MinValue ? CalculateTollFee(lastPassage, vehicle) : 0;

                if (IsWithinOneHour(lastPassage, date))
                {
                    totalFee -= tempFee;
                    totalFee += Math.Max(nextFee, tempFee);
                }
                else
                {
                    totalFee += nextFee;
                }

                if (totalFee > 60)
                {
                    totalFee = 60;
                    break;
                }

                lastPassage = date;
            }

            return totalFee;
        }

        public int CalculateTollFee(DateTime date, VehicleType vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
                return 0;

            var hour = date.Hour;
            var minute = date.Minute;

            if (isNineValue(hour, minute))
                return 9;
            else if (isSixteenValue(hour, minute))
                return 16;
            else if (isTwentyTwoValue(hour, minute))
                return 22;
            else
                return 0;
        }

        /// Flytta följande metoder till en databas?
        private static bool isTwentyTwoValue(int hour, int minute)
        {
            return (hour == 7)
                || (hour >= 9 && hour <= 14)
                || (hour == 15 && minute <= 59)
                || (hour == 16)
                || (hour == 18 && minute <= 59);
        }

        private static bool isSixteenValue(int hour, int minute)
        {
            return (hour == 6 && minute <= 59)
                || (hour == 8 && minute <= 59)
                || (hour == 15 && minute <= 29)
                || (hour == 17 && minute <= 59)
                || (hour == 18 && minute <= 29);
        }

        private static bool isNineValue(int hour, int minute)
        {
            return (hour == 6 && minute <= 29) || (hour == 8 && minute <= 29);
        }

        private bool IsTollFreeDate(DateTime date)
        {
            /// Det finns nuget paket för att hantera detta. Något man vill köra på?
            var tollFreeDates = GetTollFreeDates(date);

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            foreach (var tollDate in tollFreeDates)
            {
                /// Flytta detta till modelen istället?
                var isDayBeforeHoliday = date.AddDays(1).Date == tollDate.FullDate;
                var isTollFreeDate = date.Date.Equals(tollDate.FullDate);
                if (isDayBeforeHoliday || isTollFreeDate || date.Month == 7)
                {
                    return true;
                }
            }

            return false;
        }

        private IQueryable<TollFreeDate> GetTollFreeDates(DateTime date)
        {
            var dates = _context.TollFreeDates.Where(y => y.Year == date.Year && y.Month == date.Month);
            return dates;
        }

        private bool IsWithinOneHour(DateTime start, DateTime end)
        {
            TimeSpan timeDifference = end - start;
            return timeDifference.TotalMinutes <= 60 && timeDifference.TotalMinutes >= 0;
        }

        public bool IsTollFreeVehicle(VehicleType vehicleType)
        {
            return Enum.IsDefined(typeof(TollFreeVehicles), (int)vehicleType);
        }
    }
}
