#!/bin/bash

# deploy.sh - Simple deployment script for HamHam

echo "🚀 Starting HamHam deployment..."

# Build and start containers
docker-compose up -d --build

echo "✅ Deployment complete. Services are running in the background."
echo "DB: port 5432"
echo "Redis: port 6379"
echo "API: port 5000"
