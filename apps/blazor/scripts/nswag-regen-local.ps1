param(
    [switch]$NoPrompt
)

# Regenerate API client using the locally generated OpenAPI (no running API required)
$ErrorActionPreference = 'Stop'

# Resolve repo root
try {
    $rootDirectory = git rev-parse --show-toplevel
} catch {
    Write-Error "This script must be run inside a Git repository (git not found or not a repo)."
    exit 1
}

# Paths
$serverOpenApi = Join-Path $rootDirectory 'api/server/obj/Debug/net9.0/EndpointInfo/Server.json'
$clientNswag   = Join-Path $rootDirectory 'apps/blazor/infrastructure/Api/nswag.json'
$tempNswag     = Join-Path $env:TEMP 'nswag-local.json'

# Ensure OpenAPI file exists (build server to generate if missing)
if (-not (Test-Path $serverOpenApi)) {
    Write-Host "Local OpenAPI not found at: $serverOpenApi" -ForegroundColor Yellow
    Write-Host "Building server project to generate it..." -ForegroundColor Cyan
    pushd $rootDirectory
    try {
        dotnet build "$rootDirectory/AMIS.9.sln" --configuration Debug | Out-Null
    } finally {
        popd
    }
}

if (-not (Test-Path $serverOpenApi)) {
    Write-Error "Still cannot find local OpenAPI file: $serverOpenApi. Make sure the server project builds successfully."
    exit 1
}

# Load existing client generator config
if (-not (Test-Path $clientNswag)) {
    Write-Error "Could not find client NSwag config at: $clientNswag"
    exit 1
}

$clientConfig = Get-Content $clientNswag -Raw | ConvertFrom-Json
if (-not $clientConfig.codeGenerators) {
    Write-Error "Invalid client NSwag config: missing codeGenerators block."
    exit 1
}

# Read the local OpenAPI JSON
$openApiJson = Get-Content $serverOpenApi -Raw

# Compose a temporary NSwag config that embeds the local OpenAPI JSON
$nswagConfig = [ordered]@{
    runtime = 'Net90'
    defaultVariables = $null
    documentGenerator = @{ fromDocument = @{ json = $openApiJson; url = ''; output = $null; newLineBehavior = 'Auto' } }
    codeGenerators = $clientConfig.codeGenerators
}

$nswagConfig | ConvertTo-Json -Depth 50 | Out-File -FilePath $tempNswag -Encoding UTF8

if (-not $NoPrompt) {
    Write-Host "Using local OpenAPI file:`n  $serverOpenApi" -ForegroundColor Green
}

# Ensure local dotnet tool manifest exists
if (-not (Test-Path (Join-Path $rootDirectory '.config/dotnet-tools.json'))) {
    pushd $rootDirectory
    try {
        dotnet new tool-manifest --force | Out-Null
    } finally { popd }
}

# Ensure NSwag tool is installed
$toolList = (dotnet tool list) | Out-String
if ($toolList -notmatch 'nswag') {
    pushd $rootDirectory
    try { dotnet tool install NSwag.ConsoleCore | Out-Null } finally { popd }
}

# Run NSwag with the temporary config
pushd $rootDirectory
try {
    Write-Host "Running NSwag with offline config..." -ForegroundColor Cyan
    dotnet tool run nswag run "$tempNswag"
    Write-Host "NSwag client generation completed (offline)." -ForegroundColor Green
} finally {
    popd
}

if (-not $NoPrompt) {
    Write-Host 'Done.' -ForegroundColor Green
}
