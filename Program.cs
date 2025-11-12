using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace FairMonteCarlo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var simulation = new FairSimulation();
            List<ScenarioInput> scenarios;

            // Parse command line arguments or use default scenarios
            if (args.Length > 0)
            {
                scenarios = ParseArguments(args);
            }
            else
            {
                scenarios = GetDefaultScenarios();
            }

            // Run simulation for each scenario
            foreach (var scenarioInput in scenarios)
            {
                var scenario = CreateFullScenario(scenarioInput);
                var iterations = scenarioInput.Iterations ?? 10000;
                var results = simulation.RunSimulation(scenario, iterations);
                
                PrintResults(results);
                Console.WriteLine(new string('=', 60));
            }
        }

        private static List<ScenarioInput> ParseArguments(string[] args)
        {
            var scenarios = new List<ScenarioInput>();

            if (args[0].EndsWith(".json"))
            {
                // Load from JSON file
                var jsonContent = File.ReadAllText(args[0]);
                var collection = JsonSerializer.Deserialize<ScenarioCollection>(jsonContent);
                return collection?.Scenarios ?? new List<ScenarioInput>();
            }
            else if (args.Length >= 6)
            {
                // Parse command line: name tef_min tef_mode tef_max vuln_min vuln_mode vuln_max [iterations]
                var scenario = new ScenarioInput
                {
                    Name = args[0],
                    ThreatEventFrequency = new Distribution 
                    { 
                        Min = double.Parse(args[1]), 
                        Mode = double.Parse(args[2]), 
                        Max = double.Parse(args[3]) 
                    },
                    VulnerabilityProbability = new Distribution 
                    { 
                        Min = double.Parse(args[4]), 
                        Mode = double.Parse(args[5]), 
                        Max = double.Parse(args[6]) 
                    }
                };

                if (args.Length > 7 && int.TryParse(args[7], out int iterations))
                {
                    scenario.Iterations = iterations;
                }

                scenarios.Add(scenario);
            }
            else
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  dotnet run scenarios.json");
                Console.WriteLine("  dotnet run \"Scenario Name\" tef_min tef_mode tef_max vuln_min vuln_mode vuln_max [iterations]");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("  dotnet run \"Data Breach\" 0.1 0.3 1.2 0.1 0.4 0.8 10000");
                Environment.Exit(1);
            }

            return scenarios;
        }

        private static List<ScenarioInput> GetDefaultScenarios()
        {
            return new List<ScenarioInput>
            {
                new ScenarioInput
                {
                    Name = "Data Breach - High Risk",
                    Description = "High-frequency data breach scenario",
                    ThreatEventFrequency = new Distribution { Min = 0.5, Mode = 1.0, Max = 2.0 },
                    VulnerabilityProbability = new Distribution { Min = 0.3, Mode = 0.6, Max = 0.9 }
                },
                new ScenarioInput
                {
                    Name = "Data Breach - Medium Risk",
                    Description = "Medium-frequency data breach scenario",
                    ThreatEventFrequency = new Distribution { Min = 0.1, Mode = 0.3, Max = 1.2 },
                    VulnerabilityProbability = new Distribution { Min = 0.1, Mode = 0.4, Max = 0.8 }
                },
                new ScenarioInput
                {
                    Name = "Data Breach - Low Risk",
                    Description = "Low-frequency data breach scenario",
                    ThreatEventFrequency = new Distribution { Min = 0.05, Mode = 0.1, Max = 0.5 },
                    VulnerabilityProbability = new Distribution { Min = 0.05, Mode = 0.2, Max = 0.5 }
                }
            };
        }

        private static RiskScenario CreateFullScenario(ScenarioInput input)
        {
            // Standard loss factors (same for all scenarios)
            return new RiskScenario
            {
                Name = input.Name,
                Description = input.Description,
                ThreatEventFrequency = input.ThreatEventFrequency,
                VulnerabilityProbability = input.VulnerabilityProbability,
                
                // Standardized loss factors
                PrimaryLossFactors = new PrimaryLossFactors
                {
                    Productivity = new Distribution { Min = 50000, Mode = 150000, Max = 500000 },
                    Response = new Distribution { Min = 25000, Mode = 75000, Max = 200000 },
                    Replacement = new Distribution { Min = 10000, Mode = 30000, Max = 100000 },
                    Fines = new Distribution { Min = 100000, Mode = 500000, Max = 2000000 },
                    Judgments = new Distribution { Min = 0, Mode = 100000, Max = 1000000 },
                    CompetitiveAdvantage = new Distribution { Min = 50000, Mode = 200000, Max = 1000000 }
                },
                
                SecondaryLossFactors = new SecondaryLossFactors
                {
                    Reputation = new Distribution { Min = 100000, Mode = 300000, Max = 1500000 },
                    SecondaryResponse = new Distribution { Min = 20000, Mode = 50000, Max = 150000 }
                }
            };
        }

        private static void PrintResults(SimulationResults results)
        {
            Console.WriteLine($"=== {results.ScenarioName} ===");
            Console.WriteLine($"Iterations: {results.Iterations:N0}");
            Console.WriteLine();
            
            Console.WriteLine("Annual Loss Exposure (ALE):");
            Console.WriteLine($"  Mean: ${results.AnnualLossExposure.Mean:N0}");
            Console.WriteLine($"  Median: ${results.AnnualLossExposure.Median:N0}");
            Console.WriteLine($"  90th Percentile: ${results.AnnualLossExposure.Percentile90:N0}");
            Console.WriteLine($"  95th Percentile: ${results.AnnualLossExposure.Percentile95:N0}");
            Console.WriteLine($"  99th Percentile: ${results.AnnualLossExposure.Percentile99:N0}");
            Console.WriteLine();
            
            Console.WriteLine("Risk Components:");
            Console.WriteLine($"  Threat Event Frequency: {results.ThreatEventFrequency.Mean:F3} events/year");
            Console.WriteLine($"  Vulnerability Probability: {results.VulnerabilityProbability.Mean:F3}");
            Console.WriteLine($"  Loss Event Frequency: {results.LossEventFrequency.Mean:F3} events/year");
            Console.WriteLine();
        }
    }
}