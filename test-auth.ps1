# Trust all certificates (for development only)
add-type @"
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    public class TrustAllCertsPolicy : ICertificatePolicy {
        public bool CheckValidationResult(
            ServicePoint srvPoint, X509Certificate certificate,
            WebRequest request, int certificateProblem) {
            return true;
        }
    }
"@

[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

# Test authentication
$body = '{"email":"admin@root.com","password":"123Pa$$word!"}'
$headers = @{
    "Content-Type" = "application/json"
    "tenant"       = "root"
    "Accept"       = "application/json"
}

try {
    Write-Host "Testing authentication at https://localhost:7000/api/token/" -ForegroundColor Cyan
    $response = Invoke-RestMethod -Uri "https://localhost:7000/api/token/" -Method POST -Body $body -Headers $headers
    Write-Host "`nSUCCESS! Authentication works!" -ForegroundColor Green
    Write-Host "Token received (first 50 chars): $($response.token.Substring(0, 50))..." -ForegroundColor Green
    Write-Host "`nFull response:" -ForegroundColor Yellow
    $response | ConvertTo-Json
}
catch {
    Write-Host "`nERROR: Authentication failed!" -ForegroundColor Red
    Write-Host "Error message: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.ErrorDetails.Message) {
        Write-Host "Details: $($_.ErrorDetails.Message)" -ForegroundColor Red
    }
}
