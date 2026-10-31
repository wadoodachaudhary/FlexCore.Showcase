@description('Globally unique App Service name for Gov AI Academy. Do not use flexcore or flexcore-showcase.')
param appName string

@description('Azure region. Defaults to the resource group region.')
param location string = resourceGroup().location

@description('App Service plan SKU. B1 is enough for a pilot. Use P1v3 for a real cohort and set skuTier to PremiumV3.')
param skuName string = 'B1'

@description('SKU tier that matches skuName.')
param skuTier string = 'Basic'

@description('Authentication mode. Production should stay EntraId.')
@allowed([
  'DevelopmentMock'
  'EntraId'
])
param authMode string = 'EntraId'

@description('Entra authority. Use https://login.microsoftonline.us/ for Azure Government. Use https://login.microsoftonline.com/ only when the tenant admin confirms a commercial tenant.')
param entraInstance string = 'https://login.microsoftonline.us/'

@description('Directory (tenant) ID. Empty until the agency supplies it.')
param entraTenantId string = ''

@description('Application (client) ID. Empty until the agency supplies it.')
param entraClientId string = ''

resource plan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: '${appName}-plan'
  location: location
  sku: {
    name: skuName
    tier: skuTier
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource app 'Microsoft.Web/sites@2024-04-01' = {
  name: appName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: plan.id
    httpsOnly: true
    clientAffinityEnabled: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      alwaysOn: true
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true
      healthCheckPath: '/health'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'Authentication__Mode'
          value: authMode
        }
        {
          name: 'Authentication__Entra__Instance'
          value: entraInstance
        }
        {
          name: 'Authentication__Entra__TenantId'
          value: entraTenantId
        }
        {
          name: 'Authentication__Entra__ClientId'
          value: entraClientId
        }
      ]
    }
  }
}

output defaultHostName string = app.properties.defaultHostName
output principalId string = app.identity.principalId
