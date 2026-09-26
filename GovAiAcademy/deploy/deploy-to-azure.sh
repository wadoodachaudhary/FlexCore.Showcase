#!/usr/bin/env bash
# Zip-deploy Gov AI Academy to its own App Service.
# Requires FlexCore cloned beside this repository. See GovAiAcademy/README.md.
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
APP_NAME="${APP_NAME:-gov-ai-academy}"
RESOURCE_GROUP="${RESOURCE_GROUP:-gov-ai-academy-rg}"
PUBLISH_DIR="${ROOT}/bin/Release/net10.0/publish"
ZIP_FILE="${TMPDIR:-/tmp}/gov-ai-academy-site.zip"

case "${APP_NAME}" in
  flexcore|flexcore-showcase)
    echo "Refusing to deploy onto ${APP_NAME}. That name is reserved for FlexCore Showcase." >&2
    exit 1
    ;;
esac

if [[ "${RESOURCE_GROUP}" == "ghostwriter-prod-rg" ]]; then
  echo "Refusing to deploy into ${RESOURCE_GROUP}." >&2
  exit 1
fi

echo "Publishing Gov AI Academy (Release)"
rm -rf "${PUBLISH_DIR}"
dotnet publish "${ROOT}/GovAiAcademy.csproj" -c Release -o "${PUBLISH_DIR}"

rm -f "${ZIP_FILE}"
(cd "${PUBLISH_DIR}" && zip -qr "${ZIP_FILE}" .)

echo "Deploying to ${APP_NAME} in ${RESOURCE_GROUP}"
az webapp deploy \
  --resource-group "${RESOURCE_GROUP}" \
  --name "${APP_NAME}" \
  --src-path "${ZIP_FILE}" \
  --type zip \
  --clean true \
  --restart true

echo "Deployment finished. Check https://${APP_NAME}.azurewebsites.net/health"
