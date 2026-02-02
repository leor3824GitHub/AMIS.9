# Script to generate API client from swagger specification

Write-Host "Starting API server..." -ForegroundColor Green

# Start the API server in background
$apiProcess = Start-Process -WindowStyle Hidden -FilePath "dotnet" -ArgumentList "run", "--project", "E:\AMIS.9\api\server\Server.csproj", "--configuration", "Debug", "--no-build" -PassThru

Write-Host "Waiting for API server to start (30 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Try to fetch swagger spec
Write-Host "Fetching swagger specification..." -ForegroundColor Green

$swaggerUrl = "https://localhost:7000/swagger/v1/swagger.json"
$maxRetries = 10
$retryCount = 0

while ($retryCount -lt $maxRetries) {
    try {
        $response = Invoke-WebRequest -Uri $swaggerUrl -SkipCertificateCheck -UseBasicParsing -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            Write-Host "Successfully retrieved swagger specification!" -ForegroundColor Green
            break
        }
    } catch {
        $retryCount++
        Write-Host "Attempt $retryCount/$maxRetries - Server not ready yet, retrying in 3 seconds..." -ForegroundColor Yellow
        Start-Sleep -Seconds 3
    }
}

if ($retryCount -ge $maxRetries) {
    Write-Host "Failed to reach API server after $maxRetries attempts" -ForegroundColor Red
    Stop-Process -InputObject $apiProcess -Force
    exit 1
}

# Now run NSwag
Write-Host "Generating API client..." -ForegroundColor Green

cd "E:\AMIS.9\apps\blazor\infrastructure\Api"
& dotnet tool run nswag run ./nswag.json /variables:Configuration=Debug

# Check if generation was successful
if ($LASTEXITCODE -eq 0) {
    Write-Host "API client generated successfully!" -ForegroundColor Green
} else {
    Write-Host "API client generation failed with exit code $LASTEXITCODE" -ForegroundColor Red
}

# Stop the API server
Write-Host "Stopping API server..." -ForegroundColor Yellow
Stop-Process -InputObject $apiProcess -Force -ErrorAction SilentlyContinue

Write-Host "Done!" -ForegroundColor Green
