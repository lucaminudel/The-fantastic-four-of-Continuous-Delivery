
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
            return _parameters.IdealLapTime
                              + _parameters.DegradationPerLap * lapNumber;
        }
    }
}
