#!/bin/bash
set -euo pipefail

RESOURCE_GROUP="ghostwriter-prod-rg"
PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PUBLISH_DIR="${PROJECT_DIR}/bin/Release/net10.0/publish"
ZIP_FILE="/tmp/flexcore-site.zip"
TARGET_APPS=("flexcore" "flexcore-showcase")

echo "=================================================="
echo " Deploying FlexCore Showcase to Azure App Service"
echo " Resource Group: ${RESOURCE_GROUP}"
echo " Target Apps:    ${TARGET_APPS[*]}"
echo "=================================================="

# Step 1: Clean build & publish
echo "==> [1/3] Publishing FlexCore.Showcase in Release mode..."
rm -rf "${PUBLISH_DIR}"
dotnet publish "${PROJECT_DIR}/FlexCore.Showcase.csproj" -c Release -o "${PUBLISH_DIR}"

# Step 2: Package deployment zip
echo "==> [2/3] Packaging deployment zip..."
rm -f "${ZIP_FILE}"
(cd "${PUBLISH_DIR}" && zip -qr "${ZIP_FILE}" .)

# Step 3: Deploy to each target app
for APP in "${TARGET_APPS[@]}"; do
    echo "==> [3/3] Deploying to ${APP}.azurewebsites.net..."
    az webapp deploy \
        --resource-group "${RESOURCE_GROUP}" \
        --name "${APP}" \
        --src-path "${ZIP_FILE}" \
        --type zip \
        --clean true \
        --restart true
    echo "Deploment to ${APP} succeeded."
done

echo "All deployments completed successfully!"
