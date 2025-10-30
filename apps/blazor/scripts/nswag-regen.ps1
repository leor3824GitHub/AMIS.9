param(
	[switch]$NoPrompt
)

# Regenerate API client using the repository-level AMIS.nswag file
$ErrorActionPreference = 'Stop'

$currentDirectory = Get-Location
try {
	$rootDirectory = git rev-parse --show-toplevel
} catch {
	Write-Error "This script must be run inside a Git repository (git not found or not a repo)."
	exit 1
}

$nswagFile = Join-Path -Path $rootDirectory -ChildPath 'AMIS.nswag'
if (-not (Test-Path $nswagFile)) {
	Write-Error "Could not find AMIS.nswag at $nswagFile"
	exit 1
}

if (-not $NoPrompt) {
	Write-Host "Make sure the WebAPI is running (see $($rootDirectory)\api\server)." -ForegroundColor Yellow
	Write-Host "Press any key to continue...`n"
	$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}

# Ensure local dotnet tool manifest exists
if (-not (Test-Path (Join-Path $rootDirectory '.config\dotnet-tools.json'))) {
	dotnet new tool-manifest --force | Out-Null
}

# Ensure NSwag is installed as local tool
$toolList = dotnet tool list | Out-String
if ($toolList -notmatch 'nswag') {
	dotnet tool install NSwag.ConsoleCore | Out-Null
}

Push-Location $rootDirectory
try {
	Write-Host "Running NSwag with $nswagFile ..." -ForegroundColor Cyan
	dotnet tool run nswag run $nswagFile
	Write-Host "NSwag client generation completed." -ForegroundColor Green
}
finally {
	Pop-Location
}

if (-not $NoPrompt) {
	Write-Host -NoNewLine 'Done. Press any key to exit...';
	$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}
