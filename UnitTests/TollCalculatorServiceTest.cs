using Microsoft.EntityFrameworkCore;
using TollCalculator.Data;
using TollCalculator.Models.EF;
using TollCalculator.Models.Enums;
using TollCalculator.Service;

namespace UnitTests
{
    public class TollCalculatorServiceTests
    {
        private DbContextOptions<TollDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<TollDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void CalculateTollFee_Returns_CorrectFee()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new TollDbContext(options))
            {
                context.TollFreeDates.Add(new TollFreeDate(2023, 11, 17));
                context.SaveChanges();
            }

            using (var context = new TollDbContext(options))
            {
                var service = new TollCalculatorService(context);

                // Act
                var date = new DateTime(2023, 11, 15, 10, 30, 0);
                var tollFee = service.CalculateTollFee(date, VehicleType.Car);

                // Assert
                Assert.Equal(22, tollFee);
            }
        }

        [Fact]
        public void GetTollFee_Returns_Correct_Hour_Fee()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new TollDbContext(options))
            {
                context.TollFreeDates.Add(new TollFreeDate(2023, 11, 17));
                context.SaveChanges();
            }

            using (var context = new TollDbContext(options))
            {
                var service = new TollCalculatorService(context);

                // Act
                var date = new DateTime(2023, 11, 15, 10, 30, 0);
                var tollFee = service.CalculateTollFee(date, VehicleType.Car);

                // Assert
                Assert.Equal(22, tollFee);
            }
        }

        [Fact]
        public void GetTollFee_Returns_Zero_WhenTollFreeDate()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new TollDbContext(options))
            {
                context.TollFreeDates.Add(new TollFreeDate(2023, 11, 17));
                context.SaveChanges();
            }
            var service = new TollCalculatorService(new TollDbContext(options));

            // Act            
            var dates = new DateTime[] { new DateTime(2023, 11, 17, 8, 0, 0), new DateTime(2023, 11, 17, 17, 00, 0) };
            var tollFee = service.GetTollFee(VehicleType.Car, dates);

            // Assert            
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_Returns_Zero_WhenTollFreeVehicle()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new TollDbContext(options))
            {
                context.TollFreeDates.Add(new TollFreeDate(2023, 11, 17));
                context.SaveChanges();
            }
            var service = new TollCalculatorService(new TollDbContext(options));
            var dates = new DateTime[]
            {
                new DateTime(2023, 11, 15, 8, 0, 0),
                new DateTime(2023, 11, 15, 9, 15, 0),
                new DateTime(2023, 11, 15, 10, 30, 0)
            };

            // Act
            var tollFee = service.GetTollFee(VehicleType.Motorbike, dates);

            // Assert
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFree_ReturnsCorrectResult_WithinOneHour()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new TollDbContext(options))
            {
                context.TollFreeDates.Add(new TollFreeDate(2023, 11, 17));
                context.SaveChanges();
            }
            var service = new TollCalculatorService(new TollDbContext(options));

            // Act            
            var dates = new DateTime[] { new DateTime(2023, 11, 15, 8, 0, 0), new DateTime(2023, 11, 15, 8, 30, 0) };
            var tollFee = service.GetTollFee(VehicleType.Car, dates);

            // Assert            
            Assert.Equal(16, tollFee);
        }

        /// Mer inline data för att testa fler cases och kanske tom göra sig av med ovanstående tester.
        [Theory]
        [InlineData(new[] { "2023-11-15T08:00:00", "2023-11-15T09:15:00", "2023-11-17T10:30:00" }, new[] { "2023-11-17" }, VehicleType.Car, 31)]
        [InlineData(new[] { "2023-11-15T08:00:00", "2023-11-15T09:15:00", "2023-11-17T10:30:00" }, new[] { "2023-11-17" }, VehicleType.Motorbike, 0)]
        public void GetTollFee_Returns_CorrectTotalFee(string[] dateStrings, string[] tollFreeDateStrings, VehicleType vehicleType, int expectedTotalFee)
        {
            // Arrange
            var dates = dateStrings.Select(s => DateTime.Parse(s)).ToArray();
            var tollFreeDates = tollFreeDateStrings.Select(s => DateTime.Parse(s)).ToArray();

            var options = CreateNewContextOptions();
            using (var context = new TollDbContext(options))
            {
                foreach (var dateString in tollFreeDates)
                {
                    context.TollFreeDates.Add(new TollFreeDate(dateString.Year, dateString.Month, dateString.Day));
                }
                context.SaveChanges();
            }

            using (var context = new TollDbContext(options))
            {
                var service = new TollCalculatorService(context);

                // Act
                var totalFee = service.GetTollFee(vehicleType, dates);

                // Assert
                Assert.Equal(expectedTotalFee, totalFee);
            }
        }

        [Theory]
        [InlineData(new[] { "2023-11-15T08:00:00", "2023-11-15T09:15:00", "2023-11-15T10:30:00", "2023-11-15T11:30:00", "2023-11-15T12:30:00", "2023-11-15T14:00:00", "2023-11-15T16:00:00" }, new[] { "2023-11-17" }, VehicleType.Car, 60)]
        public void GetTollFee_Returns_Sixty_Max(string[] dateStrings, string[] tollFreeDateStrings, VehicleType vehicleType, int expectedTotalFee)
        {
            // Arrange
            var dates = dateStrings.Select(s => DateTime.Parse(s)).ToArray();
            var tollFreeDates = tollFreeDateStrings.Select(s => DateTime.Parse(s)).ToArray();

            var options = CreateNewContextOptions();
            using (var context = new TollDbContext(options))
            {
                foreach (var dateString in tollFreeDates)
                {
                    context.TollFreeDates.Add(new TollFreeDate(dateString.Year, dateString.Month, dateString.Day));
                }
                context.SaveChanges();
            }

            using (var context = new TollDbContext(options))
            {
                var service = new TollCalculatorService(context);

                // Act
                var totalFee = service.GetTollFee(vehicleType, dates);

                // Assert
                Assert.Equal(expectedTotalFee, totalFee);
            }
        }

        // Fler test för att täcka fler scenarion så som edge cases, fordons typ etc?
    }
}