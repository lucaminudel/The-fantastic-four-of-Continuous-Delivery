using NUnit.Framework;
using System.Configuration;

namespace TyresDegradationSimulator.Core.IntegrationTests
{
    public class StorageTest
    {
        string _db;
        Storage _storageV10;
        Storage _storageV11;


        [SetUp]
        public void SetUp()
        {
            _db = ConfigurationManager.AppSettings["db"];

            _storageV10 = new Storage(_db, false);
            _storageV11 = new Storage(_db, true);

        }

        [OneTimeTearDown]
        public void OneTimeTearDown() 
        {
            _storageV10.ClearParameters();
        }

        [TestCase]
        public void SupportV11_WhenEmpty()
        {
            _storageV10.ClearParameters();

            var selfConfigStorage = new Storage(_db);

            Assert.That(selfConfigStorage.SupportV11, Is.True);
        }

        [TestCase]
        public void SupportV11_WithV11()
        {
            _storageV10.ClearParameters();
            _storageV11.WriteParameters(new TyreDegradationParameters());

            var selfConfigStorage = new Storage(_db);

            Assert.That(selfConfigStorage.SupportV11, Is.True);
        }

        [TestCase]
        public void DoesNotSupportV11_WithV10()
        {
            _storageV10.ClearParameters();
            _storageV10.WriteParameters(new TyreDegradationParameters());

            var selfConfigStorage = new Storage(_db);

            Assert.That(selfConfigStorage.SupportV11, Is.False);
        }

        [TestCase]
        public void Read_v10()
        {

            _storageV10.ClearParameters();
            var tyreParameters = _storageV10.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(0));
        }

        [TestCase]
        public void Write_v10() 
        {

            const decimal idealLaptime = 1.20m;
            const decimal degradationPerLap = 0.025m;

            var tyreParameters = new TyreDegradationParameters(idealLaptime, degradationPerLap);

            _storageV10.WriteParameters(tyreParameters);
            tyreParameters = _storageV10.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(idealLaptime));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(degradationPerLap));
        }

        [TestCase]
        public void Read_v11()
        {
            _storageV11.ClearParameters();
            var tyreParameters = _storageV11.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(0));
            Assert.That(tyreParameters.DegradationPerOperatingTemperatureDelta, Is.EqualTo(0));
        }

        [TestCase]
        public void Write_v11()
        {
            const decimal idealLaptime = 1.20m;
            const decimal degradationPerLap = 0.025m;
            const decimal degradationPerOperatingTemperatureDelta = 0.06m;

            var tyreParameters = new TyreDegradationParameters(idealLaptime, degradationPerLap, degradationPerOperatingTemperatureDelta);

            _storageV11.WriteParameters(tyreParameters);
            tyreParameters = _storageV11.ReadParameters();

            Assert.That(tyreParameters.IdealLapTime, Is.EqualTo(idealLaptime));
            Assert.That(tyreParameters.DegradationPerLap, Is.EqualTo(degradationPerLap));
            Assert.That(tyreParameters.DegradationPerOperatingTemperatureDelta, Is.EqualTo(degradationPerOperatingTemperatureDelta));
        }

    }
}
