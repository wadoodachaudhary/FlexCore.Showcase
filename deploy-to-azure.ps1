# deploy-to-azure.ps1
# Deploy FlexCore.Showcase to Azure App Service from Windows PowerShell
$ErrorActionPreference = "Stop"

$ResourceGroup = "ghostwriter-prod-rg"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$PublishDir = Join-Path $ScriptDir "bin\Release\net10.0\publish"
$ZipFile = Join-Path $env:TEMP "flexcore-site.zip"
$TargetApps = @("flexcore", "flexcore-showcase")

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host " Deploying FlexCore Showcase to Azure App Service" -ForegroundColor Cyan
Write-Host " Resource Group: $ResourceGroup" -ForegroundColor Cyan
Write-Host " Target Apps:    $($TargetApps -join ', ')" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan

# Step 1: Clean build & publish
Write-Host "==> [1/3] Publishing FlexCore.Showcase in Release mode..." -ForegroundColor Yellow
if (Test-Path $PublishDir) {
    Remove-Item -Recurse -Force $PublishDir
}
dotnet publish (Join-Path $ScriptDir "FlexCore.Showcase.csproj") -c Release -o $PublishDir

# Step 2: Package deployment zip
Write-Host "==> [2/3] Packaging deployment zip..." -ForegroundColor Yellow
if (Test-Path $ZipFile) {
    Remove-Item -Force $ZipFile
}
Compress-Archive -Path "$PublishDir\*" -DestinationPath $ZipFile -Force

# Step 3: Deploy to each target app
foreach ($app in $TargetApps) {
    Write-Host "==> [3/3] Deploying to $app.azurewebsites.net..." -ForegroundColor Yellow
    az webapp deploy `
        --resource-group $ResourceGroup `
        --name $app `
        --src-path $ZipFile `
        --type zip `
        --clean true `
        --restart true
    Write-Host "Deployment to $app succeeded." -ForegroundColor Green
}

Write-Host "All deployments completed successfully!" -ForegroundColor Green
