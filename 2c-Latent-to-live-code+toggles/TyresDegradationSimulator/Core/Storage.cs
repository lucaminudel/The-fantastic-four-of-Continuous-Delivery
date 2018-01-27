using System;
using System.IO;

namespace TyresDegradationSimulator.Core
{
    public class Storage
    {
        // This Storage class is used here to represent a system component that  
        // once updated from version v10 to a backward incompatible version v11,
        // in the event of a showstopper bug it cannot be rolled back at all or
        // quickly enough to avoid a disruption for the users.
        // A component like that could be for example:
        // - a relational database with a large amount of data, updated to v11 
        //   by a DML command that cannot be easily undone.
        // or   
        // - a service in the cloud or an AWS Lambda function with an API that 
        //   changes between v10 and v11, that cannot stay live with both 
        //   versions at the same time, and with a large number of different 
        //   client apps that cannot be easily rolled back to v10 all together.

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
            try
            {
                using (var fileReader = new BinaryReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read)))
                {
                    var idealLapTime = fileReader.ReadDecimal();
                    var degradationPerLap = fileReader.ReadDecimal();

                    var degradationPerOperatingTemperatureDelta = 0m;

                    if (_temperatureSymFeatureToggle)
                    {
                        degradationPerOperatingTemperatureDelta = fileReader.ReadDecimal();
                    }

                    if (fileReader.BaseStream.Length != fileReader.BaseStream.Position) 
                    {
                        throw new InvalidOperationException("Unknown db format.");
                    }

                    return new TyreDegradationParameters(idealLapTime, degradationPerLap, degradationPerOperatingTemperatureDelta);
                }
            }
            catch (FileNotFoundException)
            {
                return new TyreDegradationParameters();
            }

        }

        public void WriteParameters(TyreDegradationParameters parameters)
        {

            using (var fileWriter = new BinaryWriter(new FileStream(_fileName, FileMode.Create, FileAccess.Write)))
            {
                fileWriter.Write(parameters.IdealLapTime);
                fileWriter.Write(parameters.DegradationPerLap);

                if (_temperatureSymFeatureToggle)
                {
                    fileWriter.Write(parameters.DegradationPerOperatingTemperatureDelta);
                }
            }
        }

        public void ClearParameters() 
        {
            File.Delete(_fileName);
        }
    }
}
