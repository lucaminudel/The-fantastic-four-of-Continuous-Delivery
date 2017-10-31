using NUnit.Framework;

namespace TyresDegradationSimulator.Core.Tests
{
    
    public class TyreDegradationSimulationTest
    {
        [TestCaseSource("VettelMonza2016Laps")]
        public void VettelMonza2016(int lapNumber, decimal expectedLapTime) 
        {
            var vettelMonza2016 = new TyreDegradationParameters(81.972m, 0.081m);
            var tyreDegradationSimulation = new TyreDegradationSimulation(vettelMonza2016);


            Assert.That(tyreDegradationSimulation.LapTime(lapNumber, 0.0m) , Is.EqualTo(expectedLapTime));
        }

        static object[] VettelMonza2016Laps =
        {
            new object[] { 5, 82.377m },
            new object[] { 6, 82.458m },
            new object[] { 7, 82.539m },
            new object[] { 8, 82.620m }
        };


        [TestCaseSource("RaikkonenMonza2016Laps")]
        public void RaikkonenMonza2016(int lapNumber, decimal expectedLapTime)
        {
            var vettelMonza2016 = new TyreDegradationParameters(82.065m, 0.081m);
            var tyreDegradationSimulation = new TyreDegradationSimulation(vettelMonza2016);


            Assert.That(tyreDegradationSimulation.LapTime(lapNumber, 0.0m), Is.EqualTo(expectedLapTime));
        }

        static object[] RaikkonenMonza2016Laps =
        {
            new object[] { 8, 82.713m },
            new object[] { 9, 82.794m }
        };

        [TestCaseSource("RaikkonenMonza2016Laps_WithOperatingTemperatureDelta")]
        public void RaikkonenMonza2016_WithOperatingTemperatureDelta(int lapNumber, decimal expectedLapTime)
        {
            var vettelMonza2016 = new TyreDegradationParameters(82.065m, 0.081m, 0.05m);
            var tyreDegradationSimulation = new TyreDegradationSimulation(vettelMonza2016);


            Assert.That(tyreDegradationSimulation.LapTime(lapNumber, 9.0m), Is.EqualTo(expectedLapTime));
        }

        static object[] RaikkonenMonza2016Laps_WithOperatingTemperatureDelta =
        {
            new object[] { 8, 83.163m },
            new object[] { 9, 83.244m }
        };

    }
}
