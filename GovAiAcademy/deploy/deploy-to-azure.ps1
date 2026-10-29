# Zip-deploy Gov AI Academy to its own App Service.
# Requires FlexCore cloned beside this repository. See GovAiAcademy/README.md.
$ErrorActionPreference = "Stop"

$Root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$AppName = if ($env:APP_NAME) { $env:APP_NAME } else { "gov-ai-academy" }
$ResourceGroup = if ($env:RESOURCE_GROUP) { $env:RESOURCE_GROUP } else { "gov-ai-academy-rg" }
$PublishDir = Join-Path $Root "bin\Release\net10.0\publish"
$ZipFile = Join-Path $env:TEMP "gov-ai-academy-site.zip"

if ($AppName -in @("flexcore", "flexcore-showcase")) {
    throw "Refusing to deploy onto $AppName. That name is reserved for FlexCore Showcase."
}
if ($ResourceGroup -eq "ghostwriter-prod-rg") {
    throw "Refusing to deploy into $ResourceGroup."
}

if (Test-Path $PublishDir) { Remove-Item -Recurse -Force $PublishDir }
dotnet publish (Join-Path $Root "GovAiAcademy.csproj") -c Release -o $PublishDir

if (Test-Path $ZipFile) { Remove-Item -Force $ZipFile }
Compress-Archive -Path (Join-Path $PublishDir "*") -DestinationPath $ZipFile -Force

az webapp deploy `
    --resource-group $ResourceGroup `
    --name $AppName `
    --src-path $ZipFile `
    --type zip `
    --clean true `
    --restart true

Write-Host "Deployment finished. Check https://$AppName.azurewebsites.net/health"
