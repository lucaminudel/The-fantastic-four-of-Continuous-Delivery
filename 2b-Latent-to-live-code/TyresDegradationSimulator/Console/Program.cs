using System.Configuration;
using TyresDegradationSimulator.Core;


namespace TyresDegradationSimulator.Console
{
    public class Program
    {
        public static void Main()
        {

            var storage = new Storage(ConfigurationManager.AppSettings["db"]);

            TyreDegradationParameters parameters = ReadTyreParameters(storage);

            RunSimulation(parameters);

        }

        private static TyreDegradationParameters ReadTyreParameters(Storage storage)
        {
            var parameters = storage.ReadParameters();

            parameters.IdealLapTime = InquireValue("Ideal Lap Time ({0} sec): ", parameters.IdealLapTime);
            parameters.DegradationPerLap = InquireValue("Degradation per Lap ({0} sec): ", parameters.DegradationPerLap);

            storage.WriteParameters(parameters);
            return parameters;
        }

        private static void RunSimulation(TyreDegradationParameters parameters)
        {
            System.Console.WriteLine("Simulation run (tyre degradation, no fuel)");
            System.Console.WriteLine("- Laps: 5-9");

            var simulation = new TyreDegradationSimulation(parameters);
            for (int lapNumber = 5; lapNumber < 10; ++lapNumber)
            {
                System.Console.WriteLine(string.Format("  Lap {0}: {1}", lapNumber, simulation.LapTime(lapNumber)));
            }
        }

        private static decimal InquireValue(string question, decimal defaultValue)
        {
            string input;
            bool success;
            decimal value;
            do
            {
                System.Console.Write(string.Format(question, defaultValue));
                input = System.Console.ReadLine();

                success = decimal.TryParse(input, out value);
            } while (!string.IsNullOrWhiteSpace(input) && !success);

            return success ? value : defaultValue;
        }

    }
}
