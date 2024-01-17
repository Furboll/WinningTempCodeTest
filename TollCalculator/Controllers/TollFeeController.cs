using Microsoft.AspNetCore.Mvc;
using TollCalculator.Models.Enums;
using TollCalculator.Service.Interface;

namespace TollCalculator.Controllers
{
    [Route("api/tollfee")]
    [ApiController]
    public class TollFeeController : ControllerBase
    {
        private readonly ITollCalculatorService _calculatorService;

        public TollFeeController(ITollCalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpPost("calculate-toll")]
        public IActionResult CalculateToll(VehicleType vehicleType, DateTime[] dates)
        {
            try
            {
                var totalPrice = _calculatorService.GetTollFee(vehicleType, dates);
                return Ok(new { TotalPrice = totalPrice });
            }
            catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}
