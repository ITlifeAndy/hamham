#!/bin/bash
set -e

echo "Building .NET project..."
dotnet build src/HamHam.sln

echo "Building React extension..."
cd src/extension
npm install
npm run build
cd ../..

echo "Build complete!"
