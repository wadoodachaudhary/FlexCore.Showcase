#!/usr/bin/env bash
# Create a dedicated resource group and App Service for Gov AI Academy.
# Does not touch flexcoreui.com or the Showcase resource group.
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
APP_NAME="${APP_NAME:-gov-ai-academy}"
RESOURCE_GROUP="${RESOURCE_GROUP:-gov-ai-academy-rg}"
LOCATION="${LOCATION:-eastus2}"
SKU_NAME="${SKU_NAME:-B1}"
SKU_TIER="${SKU_TIER:-Basic}"

case "${APP_NAME}" in
  flexcore|flexcore-showcase)
    echo "Refusing to provision ${APP_NAME}. Choose a separate app name." >&2
    exit 1
    ;;
esac

if [[ "${RESOURCE_GROUP}" == "ghostwriter-prod-rg" ]]; then
  echo "Refusing to use the Showcase resource group ${RESOURCE_GROUP}." >&2
  exit 1
fi

echo "Resource group: ${RESOURCE_GROUP}"
echo "App name:       ${APP_NAME}"
echo "Location:       ${LOCATION}"

az group create --name "${RESOURCE_GROUP}" --location "${LOCATION}" --output none

az deployment group create \
  --resource-group "${RESOURCE_GROUP}" \
  --template-file "${ROOT}/main.bicep" \
  --parameters \
    appName="${APP_NAME}" \
    skuName="${SKU_NAME}" \
    skuTier="${SKU_TIER}" \
    authMode="EntraId"

echo "Provisioned. Set the client secret later with:"
echo "  az webapp config appsettings set --resource-group ${RESOURCE_GROUP} --name ${APP_NAME} --settings Authentication__Entra__ClientSecret='<from Key Vault>'"
echo "Do not commit that value."
