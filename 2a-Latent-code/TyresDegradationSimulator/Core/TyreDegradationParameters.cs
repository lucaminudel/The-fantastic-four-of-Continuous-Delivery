namespace TyresDegradationSimulator.Core
{
    public struct TyreDegradationParameters
    {
        public decimal IdealLapTime { get; set; }
        public decimal DegradationPerLap { get; set; }
        public decimal DegradationPerOperatingTemperatureDelta { get; set; }

        public TyreDegradationParameters(decimal idealLapTime, decimal perLap)
        {
            IdealLapTime = idealLapTime;
            DegradationPerLap = perLap;
            DegradationPerOperatingTemperatureDelta = 0;
        }

        public TyreDegradationParameters(decimal idealLapTime, decimal perLap, decimal perOperatingTemperatureDelta)
        {
            IdealLapTime = idealLapTime;
            DegradationPerLap = perLap;
            DegradationPerOperatingTemperatureDelta = perOperatingTemperatureDelta;
        }

    }
}
