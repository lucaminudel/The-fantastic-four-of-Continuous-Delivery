using NUnit.Framework;
using System.Configuration;

namespace TyresDegradationSimulator.Core.IntegrationTests
{
    public class StorageTest
    {
        Storage _storage;

        bool _temperatureSymFeatureToggle;


        [SetUp]
        public void SetUp()
        {
            _temperatureSymFeatureToggle = "true" == ConfigurationManager.AppSettings["temperature_simulation_test_mode_on"];

            _storage = new Storage(ConfigurationManager.AppSettings["db"], _temperatureSymFeatureToggle);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() 
        {
            _storage.ClearParameters();
        }

        [TestCase]
        public void Read_v10()
        {

            ExecuteOnlyWithTemperatureSymFeatureToggleOff();


            _storage.ClearParameters();
            var tyreParameters = _storage.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(0));
        }

        [TestCase]
        public void Write_v10() 
        {
            ExecuteOnlyWithTemperatureSymFeatureToggleOff();

            const decimal idealLaptime = 1.20m;
            const decimal degradationPerLap = 0.025m;

            var tyreParameters = new TyreDegradationParameters(idealLaptime, degradationPerLap);

            _storage.WriteParameters(tyreParameters);
            tyreParameters = _storage.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(idealLaptime));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(degradationPerLap));
        }

        [TestCase]
        public void Read_v11()
        {
            ExecuteOnlyWithTemperatureSymFeatureToggleOn();

            _storage.ClearParameters();
            var tyreParameters = _storage.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerOperatingTemperatureDelta, Is.EqualTo(0));
        }

        [TestCase]
        public void Write_v11()
        {
            ExecuteOnlyWithTemperatureSymFeatureToggleOn();

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

        private void ExecuteOnlyWithTemperatureSymFeatureToggleOff()
        {
            if (_temperatureSymFeatureToggle)
            {
                Assert.Ignore("Temperature Sym On.");
            }
        }


        private void ExecuteOnlyWithTemperatureSymFeatureToggleOn()
        {
            if (_temperatureSymFeatureToggle == false)
            {
                Assert.Ignore("Temperature Sym Off.");
            }
        }

    }
}
