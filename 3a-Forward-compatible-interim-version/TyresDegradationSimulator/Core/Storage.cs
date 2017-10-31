using System;
using System.IO;

namespace TyresDegradationSimulator.Core
{
    public class Storage
    {
        // This file storage is used to simulate a large SQL db storage that makes it extremely
        // slow (because of the quantity of data), or difficult (because of the number of clients
        // affected) or impossible (because of irreversible schema changes) to roll back to a 
        // previous version.
        // It is an example of a change that breaks backwards compatibility that is impractical to 
        // rollback as part of an automated remediation plan.

        private string _fileName;

        public bool SupportV11 { get; private set; }

        public Storage(string db) 
        {
            _fileName = Environment.GetEnvironmentVariable("HOME")
                                   + "/Documents/F4CD_DB/"
                                   + "tyre-sym-parameters." + db;    

            try 
            {
                SupportV11 = true;
                ReadParameters();
                return;
            }
            catch(EndOfStreamException)
            {
                SupportV11 = false;
            }

        }

        public Storage(string db, bool supportV11)
            : this(db)
        {

            SupportV11 = supportV11;
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

                    if (SupportV11)
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

                if (SupportV11)
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
