
namespace TyresDegradationSimulator.Core
{
    public class TyreDegradationSimulation
    {
        private readonly TyreDegradationParameters _parameters;

        public TyreDegradationSimulation(TyreDegradationParameters parameters)
        {
            _parameters = parameters;
        }

        public decimal LapTime(int lapNumber)
        {
            return LapTime(lapNumber, 0m);
        }

        public decimal LapTime(int lapNumber, decimal operatingTemperatureDelta)
        {
            return _parameters.IdealLapTime
                              + _parameters.DegradationPerLap * lapNumber
                              + _parameters.DegradationPerOperatingTemperatureDelta * operatingTemperatureDelta;
        }
    }
}
