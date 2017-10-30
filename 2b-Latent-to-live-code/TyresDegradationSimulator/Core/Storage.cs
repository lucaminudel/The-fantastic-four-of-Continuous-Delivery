using System;
using System.IO;

namespace TyresDegradationSimulator.Core
{
    public class Storage
    {
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
