#!/bin/bash

echo "=== FAIR Monte Carlo Simulation Examples ==="
echo

echo "1. Running default scenarios..."
dotnet run
echo

echo "2. Running simple scenarios from JSON..."
dotnet run simple-scenarios.json
echo

echo "3. Running detailed scenarios from JSON..."
dotnet run scenarios.json
echo

echo "4. Running single command-line scenario..."
dotnet run "Custom Test" 0.5 1.0 2.0 0.3 0.6 0.9 5000
echo

echo "All examples completed!"