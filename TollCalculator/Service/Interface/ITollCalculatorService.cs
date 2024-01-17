using TollCalculator.Models.Enums;

namespace TollCalculator.Service.Interface
{
    public interface ITollCalculatorService
    {
        public int GetTollFee(VehicleType vehicle, DateTime[] dates);

    }
}
