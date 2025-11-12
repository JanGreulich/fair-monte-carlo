using System;

namespace FairMonteCarlo
{
    public class RiskScenario
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // FAIR Risk = Threat Event Frequency × Vulnerability × Loss Magnitude
        public Distribution ThreatEventFrequency { get; set; } = new Distribution();
        public Distribution VulnerabilityProbability { get; set; } = new Distribution();
        
        public PrimaryLossFactors PrimaryLossFactors { get; set; } = new PrimaryLossFactors();
        public SecondaryLossFactors SecondaryLossFactors { get; set; } = new SecondaryLossFactors();
    }

    public class Distribution
    {
        public double Min { get; set; }
        public double Mode { get; set; }  // Most likely value
        public double Max { get; set; }
        public DistributionType Type { get; set; } = DistributionType.Triangular;
    }

    public enum DistributionType
    {
        Triangular,
        Normal,
        LogNormal,
        Uniform
    }

    public class PrimaryLossFactors
    {
        // Direct costs from the loss event
        public Distribution Productivity { get; set; } = new Distribution();      // Lost productivity
        public Distribution Response { get; set; } = new Distribution();         // Incident response costs
        public Distribution Replacement { get; set; } = new Distribution();      // Asset replacement
        public Distribution Fines { get; set; } = new Distribution();           // Regulatory fines
        public Distribution Judgments { get; set; } = new Distribution();       // Legal judgments
        public Distribution CompetitiveAdvantage { get; set; } = new Distribution(); // Lost competitive advantage
    }

    public class SecondaryLossFactors
    {
        // Indirect costs from stakeholder reactions
        public Distribution Reputation { get; set; } = new Distribution();      // Reputation damage
        public Distribution SecondaryResponse { get; set; } = new Distribution(); // Secondary response costs
    }
}