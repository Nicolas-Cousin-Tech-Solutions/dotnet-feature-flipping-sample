#!/bin/bash

# Feature Flipping Demo Script
# This script demonstrates how feature flags work in the .NET 10 API

echo "========================================="
echo "Feature Flipping Demo - .NET 10 API"
echo "========================================="
echo ""

# Start the API in the background
echo "🚀 Starting the API..."
cd FeatureFlippingApi
dotnet run > /tmp/api.log 2>&1 &
API_PID=$!
cd ..

# Wait for API to start
sleep 5

echo "✅ API started on http://localhost:5078"
echo ""

# Test health endpoint (always available)
echo "📡 Testing Health Check (always available)..."
curl -s http://localhost:5078/api/health | jq .
echo ""

# Test weather endpoint (enabled in Development)
echo "🌤️  Testing Weather Forecast (EnableWeatherForecast=true in Development)..."
HTTP_CODE=$(curl -s -o /tmp/weather.json -w "%{http_code}" http://localhost:5078/api/weather)
echo "HTTP Status: $HTTP_CODE"
if [ "$HTTP_CODE" -eq 200 ]; then
    echo "✅ Feature is ENABLED"
    jq '.[0]' /tmp/weather.json
else
    echo "❌ Feature is DISABLED"
fi
echo ""

# Test products endpoint (disabled in Development)
echo "🛒 Testing Product Catalog (EnableProductCatalog=false in Development)..."
HTTP_CODE=$(curl -s -o /tmp/products.json -w "%{http_code}" http://localhost:5078/api/products)
echo "HTTP Status: $HTTP_CODE"
if [ "$HTTP_CODE" -eq 200 ]; then
    echo "✅ Feature is ENABLED"
    jq '.[0]' /tmp/products.json
else
    echo "❌ Feature is DISABLED (503 Service Unavailable)"
fi
echo ""

# Stop the API
echo "🛑 Stopping the API..."
kill $API_PID
wait $API_PID 2>/dev/null

echo ""
echo "========================================="
echo "Demo completed!"
echo ""
echo "📝 To change feature flags, edit:"
echo "   - appsettings.json (production)"
echo "   - appsettings.Development.json (development)"
echo ""
echo "🧪 Run tests with: dotnet test"
echo "📊 View presentation with: reveal-md docs/slides.md"
echo "========================================="
