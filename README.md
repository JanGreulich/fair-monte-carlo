# FAIR Monte Carlo Simulation

A C# implementation of Monte Carlo simulation for FAIR (Factor Analysis of Information Risk) quantitative risk analysis.

## What is FAIR?

FAIR is a quantitative risk analysis methodology that breaks down risk into:
- **Risk = Threat Event Frequency × Vulnerability × Loss Magnitude**

Where:
- **Threat Event Frequency (TEF)**: How often a threat actor acts against an asset
- **Vulnerability**: Probability that a threat event results in loss
- **Loss Magnitude**: Financial impact when a loss occurs (Primary + Secondary losses)

## Features

- Monte Carlo simulation with configurable iterations
- Support for multiple probability distributions (Triangular, Normal, LogNormal, Uniform)
- Comprehensive FAIR loss factor modeling
- Statistical analysis with percentiles
- JSON output for further analysis
- Docker containerization

## Input Parameters

### Risk Scenario Configuration

```csharp
var scenario = new RiskScenario
{
    Name = "Data Breach Risk",
    ThreatEventFrequency = new Distribution { Min = 0.1, Mode = 0.3, Max = 1.2 }, // events/year
    VulnerabilityProbability = new Distribution { Min = 0.1, Mode = 0.4, Max = 0.8 },
    
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
```

### Distribution Types

- **Triangular**: Min, Mode (most likely), Max values
- **Normal**: Mean and standard deviation
- **LogNormal**: For skewed distributions
- **Uniform**: Equal probability between min and max

## Output

### Console Output
```
=== FAIR Monte Carlo Simulation Results ===
Scenario: Customer Data Breach
Iterations: 10,000

Annual Loss Exposure (ALE):
  Mean: $486,234
  Median: $312,567
  90th Percentile: $1,234,567
  95th Percentile: $1,876,543
  99th Percentile: $3,456,789

Risk Components:
  Threat Event Frequency: 0.533 events/year
  Vulnerability Probability: 0.467
  Loss Event Frequency: 0.249 events/year

Loss Magnitude Breakdown:
  Primary Losses: $1,234,567
  Secondary Losses: $456,789
  Total Loss Magnitude: $1,691,356
```

### JSON Output (simulation_results.json)
```json
{
  "ScenarioName": "Customer Data Breach",
  "Iterations": 10000,
  "Timestamp": "2024-01-15T10:30:00Z",
  "AnnualLossExposure": {
    "Mean": 486234.56,
    "Median": 312567.89,
    "StandardDeviation": 234567.12,
    "Percentile90": 1234567.89,
    "Percentile95": 1876543.21,
    "Percentile99": 3456789.01
  },
  "PrimaryLossBreakdown": {
    "Productivity": { "Mean": 150000, "Median": 145000 },
    "Response": { "Mean": 75000, "Median": 72000 },
    "Fines": { "Mean": 500000, "Median": 450000 }
  }
}
```

## Usage

### 1. Default Scenarios (No Arguments)
```bash
dotnet run
```
Runs three built-in scenarios: High, Medium, and Low risk data breach scenarios.

### 2. JSON Input File
```bash
dotnet run scenarios.json
dotnet run simple-scenarios.json
```

### 3. Command Line Arguments
```bash
dotnet run "Scenario Name" tef_min tef_mode tef_max vuln_min vuln_mode vuln_max [iterations]
```

**Example:**
```bash
dotnet run "Ransomware Attack" 1.0 2.0 4.0 0.6 0.8 0.95 5000
```

### 4. Run All Examples
```bash
./run-examples.sh
```

## Podman Usage

### Build and Run
```bash
# Build the image
podman build -t fair-monte-carlo .

# Run with default scenarios
podman run --rm fair-monte-carlo

# Run with JSON file
podman run --rm -v $(pwd):/app/input fair-monte-carlo input/scenarios.json

# Run with command line args
podman run --rm fair-monte-carlo "Test Scenario" 0.5 1.0 2.0 0.3 0.6 0.9 5000
```

### Using Podman Compose
```bash
# Run simple scenarios
podman-compose up fair-simple

# Run custom scenario
podman-compose up fair-custom

# Run default scenarios
podman-compose up fair-simulation
```

### Podman Setup (One-time)
```bash
# Install Podman
brew install podman podman-compose

# Initialize and start Podman machine
podman machine init
podman machine start

# Verify installation
podman --version
podman run --rm hello-world
```

## Use Cases

1. **Cybersecurity Risk Assessment**
   - Data breach scenarios
   - Ransomware impact analysis
   - Insider threat quantification

2. **Compliance Risk**
   - Regulatory fine exposure
   - Audit failure costs
   - Privacy violation impacts

3. **Operational Risk**
   - System downtime costs
   - Supply chain disruptions
   - Business continuity planning

4. **Investment Decisions**
   - Security control ROI analysis
   - Risk transfer vs. mitigation
   - Budget allocation optimization

## Recommended Inputs by Scenario Type

### Data Breach
- TEF: 0.1-2.0 events/year (based on industry data)
- Vulnerability: 0.2-0.8 (depends on security maturity)
- Fines: $100K-$10M (GDPR, CCPA penalties)
- Reputation: 10-50% of annual revenue

### Ransomware
- TEF: 0.5-3.0 events/year (high frequency)
- Vulnerability: 0.3-0.9 (email/endpoint exposure)
- Response: $50K-$500K (incident response)
- Downtime: $10K-$1M per day

### Insider Threat
- TEF: 0.1-1.0 events/year (lower frequency)
- Vulnerability: 0.4-0.8 (privileged access)
- Competitive Advantage: High impact
- Legal costs: Significant for IP theft

## Next Steps for Discussion

1. **Scenario Customization**: What specific risk scenarios do you want to model?
2. **Data Sources**: How will you gather input parameters (expert judgment, historical data, industry benchmarks)?
3. **Integration**: Do you need API endpoints, database connectivity, or specific output formats?
4. **Visualization**: Should we add charts, graphs, or dashboard capabilities?
5. **Advanced Features**: Risk correlation, scenario comparison, sensitivity analysis?