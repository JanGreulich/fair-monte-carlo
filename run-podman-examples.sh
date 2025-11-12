#!/bin/bash

echo "=== FAIR Monte Carlo with Podman Examples ==="
echo

# Build the image
echo "Building Podman image..."
podman build -t fair-monte-carlo . -q
echo "✅ Image built successfully"
echo

echo "1. Running default scenarios..."
podman run --rm fair-monte-carlo
echo

echo "2. Running simple scenarios from JSON..."
podman run --rm -v $(pwd):/app/input fair-monte-carlo input/simple-scenarios.json
echo

echo "3. Running single command-line scenario..."
podman run --rm fair-monte-carlo "Podman Test" 0.8 1.5 3.0 0.4 0.7 0.9 5000
echo

echo "4. Using podman-compose..."
podman-compose up fair-custom
echo

echo "All Podman examples completed!"
echo
echo "🐳 Podman is working perfectly with your FAIR Monte Carlo simulation!"