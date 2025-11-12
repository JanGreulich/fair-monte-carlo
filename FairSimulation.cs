using System;
using System.Collections.Generic;
using System.Linq;

namespace FairMonteCarlo
{
    public class FairSimulation
    {
        private readonly Random _random;

        public FairSimulation(int? seed = null)
        {
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public SimulationResults RunSimulation(RiskScenario scenario, int iterations = 10000)
        {
            var results = new SimulationResults
            {
                ScenarioName = scenario.Name,
                Iterations = iterations
            };

            var aleValues = new List<double>();
            var tefValues = new List<double>();
            var vulnValues = new List<double>();
            var lefValues = new List<double>();
            var primaryLossValues = new List<double>();
            var secondaryLossValues = new List<double>();
            var totalLossValues = new List<double>();

            // Track individual loss factors
            var primaryBreakdown = new Dictionary<string, List<double>>
            {
                ["Productivity"] = new List<double>(),
                ["Response"] = new List<double>(),
                ["Replacement"] = new List<double>(),
                ["Fines"] = new List<double>(),
                ["Judgments"] = new List<double>(),
                ["CompetitiveAdvantage"] = new List<double>()
            };

            var secondaryBreakdown = new Dictionary<string, List<double>>
            {
                ["Reputation"] = new List<double>(),
                ["SecondaryResponse"] = new List<double>()
            };

            for (int i = 0; i < iterations; i++)
            {
                // Sample FAIR components
                var tef = SampleDistribution(scenario.ThreatEventFrequency);
                var vuln = SampleDistribution(scenario.VulnerabilityProbability);
                var lef = tef * vuln; // Loss Event Frequency

                // Sample primary loss factors
                var productivity = SampleDistribution(scenario.PrimaryLossFactors.Productivity);
                var response = SampleDistribution(scenario.PrimaryLossFactors.Response);
                var replacement = SampleDistribution(scenario.PrimaryLossFactors.Replacement);
                var fines = SampleDistribution(scenario.PrimaryLossFactors.Fines);
                var judgments = SampleDistribution(scenario.PrimaryLossFactors.Judgments);
                var competitive = SampleDistribution(scenario.PrimaryLossFactors.CompetitiveAdvantage);

                var primaryLoss = productivity + response + replacement + fines + judgments + competitive;

                // Sample secondary loss factors
                var reputation = SampleDistribution(scenario.SecondaryLossFactors.Reputation);
                var secondaryResponse = SampleDistribution(scenario.SecondaryLossFactors.SecondaryResponse);

                var secondaryLoss = reputation + secondaryResponse;
                var totalLoss = primaryLoss + secondaryLoss;

                // Calculate Annual Loss Exposure (ALE)
                var ale = lef * totalLoss;

                // Store values
                aleValues.Add(ale);
                tefValues.Add(tef);
                vulnValues.Add(vuln);
                lefValues.Add(lef);
                primaryLossValues.Add(primaryLoss);
                secondaryLossValues.Add(secondaryLoss);
                totalLossValues.Add(totalLoss);

                // Store breakdown values
                primaryBreakdown["Productivity"].Add(productivity);
                primaryBreakdown["Response"].Add(response);
                primaryBreakdown["Replacement"].Add(replacement);
                primaryBreakdown["Fines"].Add(fines);
                primaryBreakdown["Judgments"].Add(judgments);
                primaryBreakdown["CompetitiveAdvantage"].Add(competitive);

                secondaryBreakdown["Reputation"].Add(reputation);
                secondaryBreakdown["SecondaryResponse"].Add(secondaryResponse);
            }

            // Calculate statistical summaries
            results.AnnualLossExposure = CalculateStatistics(aleValues);
            results.ThreatEventFrequency = CalculateStatistics(tefValues);
            results.VulnerabilityProbability = CalculateStatistics(vulnValues);
            results.LossEventFrequency = CalculateStatistics(lefValues);
            results.PrimaryLossMagnitude = CalculateStatistics(primaryLossValues);
            results.SecondaryLossMagnitude = CalculateStatistics(secondaryLossValues);
            results.TotalLossMagnitude = CalculateStatistics(totalLossValues);

            // Calculate breakdown statistics
            foreach (var kvp in primaryBreakdown)
            {
                results.PrimaryLossBreakdown[kvp.Key] = CalculateStatistics(kvp.Value);
            }

            foreach (var kvp in secondaryBreakdown)
            {
                results.SecondaryLossBreakdown[kvp.Key] = CalculateStatistics(kvp.Value);
            }

            results.RawAnnualLossExposure = aleValues;

            return results;
        }

        private double SampleDistribution(Distribution distribution)
        {
            return distribution.Type switch
            {
                DistributionType.Triangular => SampleTriangular(distribution.Min, distribution.Mode, distribution.Max),
                DistributionType.Normal => SampleNormal(distribution.Mode, (distribution.Max - distribution.Min) / 6),
                DistributionType.LogNormal => SampleLogNormal(distribution.Mode, (distribution.Max - distribution.Min) / 6),
                DistributionType.Uniform => SampleUniform(distribution.Min, distribution.Max),
                _ => SampleTriangular(distribution.Min, distribution.Mode, distribution.Max)
            };
        }

        private double SampleTriangular(double min, double mode, double max)
        {
            var u = _random.NextDouble();
            var c = (mode - min) / (max - min);

            if (u < c)
            {
                return min + Math.Sqrt(u * (max - min) * (mode - min));
            }
            else
            {
                return max - Math.Sqrt((1 - u) * (max - min) * (max - mode));
            }
        }

        private double SampleNormal(double mean, double stdDev)
        {
            // Box-Muller transform
            var u1 = 1.0 - _random.NextDouble();
            var u2 = 1.0 - _random.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        private double SampleLogNormal(double mean, double stdDev)
        {
            var normal = SampleNormal(Math.Log(mean), stdDev);
            return Math.Exp(normal);
        }

        private double SampleUniform(double min, double max)
        {
            return min + _random.NextDouble() * (max - min);
        }

        private StatisticalSummary CalculateStatistics(List<double> values)
        {
            var sorted = values.OrderBy(x => x).ToList();
            var count = sorted.Count;

            return new StatisticalSummary
            {
                Mean = values.Average(),
                Median = GetPercentile(sorted, 0.5),
                StandardDeviation = Math.Sqrt(values.Select(x => Math.Pow(x - values.Average(), 2)).Average()),
                Min = sorted.First(),
                Max = sorted.Last(),
                Percentile10 = GetPercentile(sorted, 0.1),
                Percentile25 = GetPercentile(sorted, 0.25),
                Percentile75 = GetPercentile(sorted, 0.75),
                Percentile90 = GetPercentile(sorted, 0.9),
                Percentile95 = GetPercentile(sorted, 0.95),
                Percentile99 = GetPercentile(sorted, 0.99)
            };
        }

        private double GetPercentile(List<double> sortedValues, double percentile)
        {
            var index = percentile * (sortedValues.Count - 1);
            var lower = (int)Math.Floor(index);
            var upper = (int)Math.Ceiling(index);

            if (lower == upper)
                return sortedValues[lower];

            var weight = index - lower;
            return sortedValues[lower] * (1 - weight) + sortedValues[upper] * weight;
        }
    }
}