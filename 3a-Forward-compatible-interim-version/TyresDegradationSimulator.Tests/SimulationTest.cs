using System;
using System.IO;
using System.Configuration;
using NUnit.Framework;

namespace TyresDegradationSimulator.Console.AcceptanceTests
{
    
    public class SimulationTest
    {

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            new Core.Storage(ConfigurationManager.AppSettings["db"]).ClearParameters();
        }

        string RunProgram(params string[] inputList)
        {
            using (var input = new StringReader(string.Join(Environment.NewLine, inputList)))
            using (var output = new StringWriter())
            {

                System.Console.SetIn(input);
                System.Console.SetOut(output);
                Program.Main();

                return output.ToString();
            }
        }

        [Test]
        public void VettelMonza2016()
        {
            SetUpStorageV10();

            var output = RunProgram("81.972", "0.081");
            Assert.That(output, Does.EndWith("Lap 5: 82.377\n  Lap 6: 82.458\n  Lap 7: 82.539\n  Lap 8: 82.620\n  Lap 9: 82.701\n"));
        }


        [Test]
        public void VettelMonza2016_UsingStoredParameters()
        {
            SetUpStorageV10();

            RunProgram("81.972", "0.081");
            var output = RunProgram("", "", "", "0");
            Assert.That(output, Does.EndWith("Lap 5: 82.377\n  Lap 6: 82.458\n  Lap 7: 82.539\n  Lap 8: 82.620\n  Lap 9: 82.701\n"));
        }

        [Test]
        public void RaikkonenMonza2016()
        {
              
            SetUpStorageV10();

            var output = RunProgram("82.065", "0.081");
            Assert.That(output, Does.EndWith("Lap 5: 82.470\n  Lap 6: 82.551\n  Lap 7: 82.632\n  Lap 8: 82.713\n  Lap 9: 82.794\n"));
        }

        [Test]
        public void VettelMonza2016_WithTemperatureSym()
        {
            SetupStorageV11();

            var output = RunProgram("81.972", "0.081", "0.05", "0");
            Assert.That(output, Does.EndWith("Lap 5: 82.377\n  Lap 6: 82.458\n  Lap 7: 82.539\n  Lap 8: 82.620\n  Lap 9: 82.701\n"));
        }


        [Test]
        public void RaikkonenMonza2016_WithTemperatureSym()
        {
            SetupStorageV11();

            var output = RunProgram("82.065", "0.081", "0.05", "9");
            Assert.That(output, Does.EndWith("Lap 5: 82.920\n  Lap 6: 83.001\n  Lap 7: 83.082\n  Lap 8: 83.163\n  Lap 9: 83.244\n"));
        }


        [Test]
        public void RaikkonenMonza2016_UsingStoredParameters_WithTemperatureSym()
        {
            SetupStorageV11();

            RunProgram("82.065", "0.081", "0.05", "9");
            var output = RunProgram("", "", "", "9");
            Assert.That(output, Does.EndWith("Lap 5: 82.920\n  Lap 6: 83.001\n  Lap 7: 83.082\n  Lap 8: 83.163\n  Lap 9: 83.244\n"));
        }


        private void SetUpStorageV10()
        {
            SetupStorage(ConfigurationManager.AppSettings["db"], false);
        }


        private void SetupStorageV11()
        {
            SetupStorage(ConfigurationManager.AppSettings["db"], true);
        }

        private static void SetupStorage(string db, bool supportV11)
        {
            var storage = new Core.Storage(db, supportV11);
            storage.ClearParameters();
            storage.WriteParameters(new Core.TyreDegradationParameters());
        }
    }
}
