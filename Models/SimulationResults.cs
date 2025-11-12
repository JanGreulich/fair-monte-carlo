using System;
using System.Collections.Generic;

namespace FairMonteCarlo
{
    public class SimulationResults
    {
        public string ScenarioName { get; set; } = string.Empty;
        public int Iterations { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        // Key FAIR metrics
        public StatisticalSummary AnnualLossExposure { get; set; } = new StatisticalSummary();
        public StatisticalSummary ThreatEventFrequency { get; set; } = new StatisticalSummary();
        public StatisticalSummary VulnerabilityProbability { get; set; } = new StatisticalSummary();
        public StatisticalSummary LossEventFrequency { get; set; } = new StatisticalSummary();
        
        // Loss magnitude components
        public StatisticalSummary PrimaryLossMagnitude { get; set; } = new StatisticalSummary();
        public StatisticalSummary SecondaryLossMagnitude { get; set; } = new StatisticalSummary();
        public StatisticalSummary TotalLossMagnitude { get; set; } = new StatisticalSummary();
        
        // Detailed breakdowns
        public Dictionary<string, StatisticalSummary> PrimaryLossBreakdown { get; set; } = new Dictionary<string, StatisticalSummary>();
        public Dictionary<string, StatisticalSummary> SecondaryLossBreakdown { get; set; } = new Dictionary<string, StatisticalSummary>();
        
        // Raw data for further analysis
        public List<double> RawAnnualLossExposure { get; set; } = new List<double>();
    }

    public class StatisticalSummary
    {
        public double Mean { get; set; }
        public double Median { get; set; }
        public double StandardDeviation { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Percentile10 { get; set; }
        public double Percentile25 { get; set; }
        public double Percentile75 { get; set; }
        public double Percentile90 { get; set; }
        public double Percentile95 { get; set; }
        public double Percentile99 { get; set; }
    }
}