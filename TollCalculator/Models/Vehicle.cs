using TollCalculator.Models.Enums;
using TollCalculator.Models.Interface;

namespace TollCalculator.Models
{
    public class Vehicle : IVechicle
    {
        public Enum GetVehicleType(int type)
        {
            switch (type)
            {
                case 1:
                    return VehicleType.Car;
                case 2:
                    return VehicleType.Motorbike;
                case 3:
                    return VehicleType.Tractor;
                case 4:
                    return VehicleType.Emergency;
                case 5:
                    return VehicleType.Diplomat;
                case 6:
                    return VehicleType.Foreign;
                case 7:
                    return VehicleType.Military;
                case 8:
                    return VehicleType.Truck;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Type does not exist.");
            }
        }
    }
}
