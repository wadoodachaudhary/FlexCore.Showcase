# Create a dedicated resource group and App Service for Gov AI Academy.
# Does not touch flexcoreui.com or the Showcase resource group.
$ErrorActionPreference = "Stop"

$Root = Split-Path -Parent $MyInvocation.MyCommand.Path
$AppName = if ($env:APP_NAME) { $env:APP_NAME } else { "gov-ai-academy" }
$ResourceGroup = if ($env:RESOURCE_GROUP) { $env:RESOURCE_GROUP } else { "gov-ai-academy-rg" }
$Location = if ($env:LOCATION) { $env:LOCATION } else { "eastus2" }
$SkuName = if ($env:SKU_NAME) { $env:SKU_NAME } else { "B1" }
$SkuTier = if ($env:SKU_TIER) { $env:SKU_TIER } else { "Basic" }

if ($AppName -in @("flexcore", "flexcore-showcase")) {
    throw "Refusing to provision $AppName. Choose a separate app name."
}
if ($ResourceGroup -eq "ghostwriter-prod-rg") {
    throw "Refusing to use the Showcase resource group $ResourceGroup."
}

Write-Host "Resource group: $ResourceGroup"
Write-Host "App name:       $AppName"

az group create --name $ResourceGroup --location $Location --output none
az deployment group create `
    --resource-group $ResourceGroup `
    --template-file (Join-Path $Root "main.bicep") `
    --parameters appName=$AppName skuName=$SkuName skuTier=$SkuTier authMode=EntraId

Write-Host "Provisioned. Set the client secret later. Do not commit it."
Write-Host "az webapp config appsettings set --resource-group $ResourceGroup --name $AppName --settings Authentication__Entra__ClientSecret=<from Key Vault>"
