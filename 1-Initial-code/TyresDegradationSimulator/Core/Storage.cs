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

        public Storage(string db)
        {

            _fileName = Environment.GetEnvironmentVariable("HOME") 
                                   + "/Documents/F4CD_DB/" 
                                   + "tyre-sym-parameters." + db;

        }

        public TyreDegradationParameters ReadParameters()
        {

            try
            {
                using (var fileReader = new BinaryReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read)))
                {
                    var idealLapTime = fileReader.ReadDecimal();
                    var degradationPerLap = fileReader.ReadDecimal();

                    if (fileReader.BaseStream.Length != fileReader.BaseStream.Position)
                    {
                        throw new InvalidOperationException("Unknown db format.");
                    }

                    return new TyreDegradationParameters(idealLapTime, degradationPerLap);

                }
            }
            catch(FileNotFoundException) 
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
            }
        }

        public void ClearParameters() 
        {
            File.Delete(_fileName);
        }
    }
}
