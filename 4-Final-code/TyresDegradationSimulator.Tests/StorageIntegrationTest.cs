using NUnit.Framework;
using System.Configuration;

namespace TyresDegradationSimulator.Core.IntegrationTests
{
    public class StorageTest
    {
        Storage _storage;


        [SetUp]
        public void SetUp()
        {
            _storage = new Storage(ConfigurationManager.AppSettings["db"]);

        }

        [OneTimeTearDown]
        public void OneTimeTearDown() 
        {
            _storage.ClearParameters();
        }


        [TestCase]
        public void ReadWhenEmpty()
        {
            _storage.ClearParameters();
            var tyreParameters = _storage.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerOperatingTemperatureDelta, Is.EqualTo(0));
        }

        [TestCase]
        public void WriteAndRead()
        {
            const decimal idealLaptime = 1.20m;
            const decimal degradationPerLap = 0.025m;
            const decimal degradationPerOperatingTemperatureDelta = 0.06m;

            var tyreParameters = new TyreDegradationParameters(idealLaptime, degradationPerLap, degradationPerOperatingTemperatureDelta);

            _storage.WriteParameters(tyreParameters);
            tyreParameters = _storage.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(idealLaptime));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(degradationPerLap));
            Assert.That(tyreParameters.DegradationPerOperatingTemperatureDelta, Is.EqualTo(degradationPerOperatingTemperatureDelta));
        }

    }
}
