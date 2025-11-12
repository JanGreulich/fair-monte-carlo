using System;
using System.Collections.Generic;

namespace FairMonteCarlo
{
    public class ScenarioInput
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Only customizable parameters
        public Distribution ThreatEventFrequency { get; set; } = new Distribution();
        public Distribution VulnerabilityProbability { get; set; } = new Distribution();
        
        // Optional: Override iterations
        public int? Iterations { get; set; }
    }

    public class ScenarioCollection
    {
        public List<ScenarioInput> Scenarios { get; set; } = new List<ScenarioInput>();
        public int DefaultIterations { get; set; } = 10000;
    }
}