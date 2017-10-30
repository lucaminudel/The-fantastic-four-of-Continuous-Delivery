using System.Configuration;
using TyresDegradationSimulator.Core;


namespace TyresDegradationSimulator.Console
{
    public class Program
    {
        public static void Main()
        {

            bool temperatureSymFeatureToggle = "true" == ConfigurationManager.AppSettings["temperature_simulation_test_mode_on"];

            var storage = new Storage(ConfigurationManager.AppSettings["db"], temperatureSymFeatureToggle);

            TyreDegradationParameters parameters = ReadTyreParameters(storage, temperatureSymFeatureToggle);

            decimal temperatureDelta = ReadSimulationParameters(temperatureSymFeatureToggle);

            RunSimulation(parameters, temperatureDelta);

        }

        private static TyreDegradationParameters ReadTyreParameters(Storage storage, bool temperatureSymFeatureToggle)
        {
            var parameters = storage.ReadParameters();

            parameters.IdealLapTime = InquireValue("Ideal Lap Time ({0} sec): ", parameters.IdealLapTime);
            parameters.DegradationPerLap = InquireValue("Degradation per Lap ({0} sec): ", parameters.DegradationPerLap);

            if (temperatureSymFeatureToggle) 
            {
                parameters.DegradationPerOperatingTemperatureDelta = InquireValue("Degradation per operating temperature delta ({0} sec): ", parameters.DegradationPerOperatingTemperatureDelta);
            }

            storage.WriteParameters(parameters);
            return parameters;
        }

        private static decimal ReadSimulationParameters(bool temperatureSymFeatureToggle)
        {
            if (!temperatureSymFeatureToggle) { return 0m; }

            System.Console.WriteLine();
            decimal temperatureDelta = InquireValue("Simulation operating temperature delta ({0} degrees): ", 0);
            return temperatureDelta;
        }

        private static void RunSimulation(TyreDegradationParameters parameters, decimal temperatureDelta)
        {
            System.Console.WriteLine("Simulation run (tyre degradation, no fuel)");
            System.Console.WriteLine("- Laps: 5-9");

            var simulation = new TyreDegradationSimulation(parameters);
            for (int lapNumber = 5; lapNumber < 10; ++lapNumber)
            {
                System.Console.WriteLine(string.Format("  Lap {0}: {1}", lapNumber, simulation.LapTime(lapNumber, temperatureDelta)));
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
