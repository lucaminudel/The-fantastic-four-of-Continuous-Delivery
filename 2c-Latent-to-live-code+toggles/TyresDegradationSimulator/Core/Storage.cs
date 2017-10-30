using System;
using System.IO;

namespace TyresDegradationSimulator.Core
{
    public class Storage
    {
        private string _fileName;
        readonly bool _temperatureSymFeatureToggle;

        public Storage(string db, bool temperatureSymFeatureToggle)
        {
            _fileName = Environment.GetEnvironmentVariable("HOME") 
                                   + "/Documents/F4CD_DB/" 
                                   + "tyre-sym-parameters." + db;

            _temperatureSymFeatureToggle = temperatureSymFeatureToggle;
        }

        public TyreDegradationParameters ReadParameters()
        {
            if (_temperatureSymFeatureToggle) 
            {
                return ReadParameters_v11();
            }

            try
            {
                using (var fileReader = new BinaryReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read)))
                {
                    var idealLapTime = fileReader.ReadDecimal();
                    var degradationPerLap = fileReader.ReadDecimal();

                    return new TyreDegradationParameters(idealLapTime, degradationPerLap);

                }
            }
            catch (FileNotFoundException)
            {
                return new TyreDegradationParameters();
            }

        }

        public void WriteParameters(TyreDegradationParameters parameters)
        {

            if (_temperatureSymFeatureToggle)
            {
                WriteParameters_v11(parameters);
                return;
            }

            using (var fileWriter = new BinaryWriter(new FileStream(_fileName, FileMode.Create, FileAccess.Write)))
            {
                fileWriter.Write(parameters.IdealLapTime);
                fileWriter.Write(parameters.DegradationPerLap);
            }
        }


        private void WriteParameters_v11(TyreDegradationParameters parameters)
        {
            using (var fileWriter = new BinaryWriter(new FileStream(_fileName, FileMode.Create, FileAccess.Write)))
            {
                fileWriter.Write(parameters.IdealLapTime);
                fileWriter.Write(parameters.DegradationPerLap);
                fileWriter.Write(parameters.DegradationPerOperatingTemperatureDelta);
            }
        }


        private TyreDegradationParameters ReadParameters_v11()
        {

            try
            {
                using (var fileReader = new BinaryReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read)))
                {
                    var idealLapTime = fileReader.ReadDecimal();
                    var degradationPerLap = fileReader.ReadDecimal();
                    var degradationPerOperatingTemperatureDelta = fileReader.ReadDecimal();

                    return new TyreDegradationParameters(idealLapTime, degradationPerLap, degradationPerOperatingTemperatureDelta);

                }
            }
            catch (FileNotFoundException)
            {
                return new TyreDegradationParameters();
            }
        }



        public void ClearParameters() 
        {
            File.Delete(_fileName);
        }
    }
}
