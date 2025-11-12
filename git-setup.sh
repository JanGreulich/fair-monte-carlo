#!/bin/bash

echo "Setting up FAIR Monte Carlo Git Repository..."

# Initialize git repository
git init

# Add all source files
git add .gitignore
git add README.md
git add *.csproj
git add *.cs
git add Models/
git add Dockerfile
git add docker-compose.yml
git add *.json
git add *.sh

# Initial commit
git commit -m "Initial commit: FAIR Monte Carlo simulation

- C# Monte Carlo simulation for FAIR risk analysis
- Support for multiple input methods (CLI, JSON, default scenarios)
- Podman/Docker containerization
- Standardized loss factors with customizable threat/vulnerability parameters
- Statistical analysis with percentiles and risk breakdowns"

echo "✅ Git repository initialized and committed!"
echo
echo "Next steps:"
echo "1. Create repository on GitHub/GitLab"
echo "2. git remote add origin <your-repo-url>"
echo "3. git push -u origin main"
echo
echo "Your colleague can then:"
echo "git clone <your-repo-url>"
echo "cd fair-monte-carlo"
echo "./run-examples.sh          # For .NET"
echo "./run-podman-examples.sh   # For Podman"