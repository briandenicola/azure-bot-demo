# Introduction

Bot Framework bot sample with Azure AD Authentication

# Setup
## Manual Steps
    * Create Azure AD Service Principal for Bot
        * $botPass = New-Password -Length 25 (Function from bjd.Common.Functions)
        * $botAppId = $(az ad sp create-for-rbac --name bjdBotApp01 --query 'appId' -o tsv) 
        * az ad sp credential reset --name bjdBotApp01 --password $botPass --credential-description "default" --years 2
    * Create Azure AD Service Principal for Bot Authentication 
        * $botAuthPass = New-Password -Length 25 (Function from bjd.Common.Functions)
        * $botAuthId = $(az ad app create --display-name bjdBotAuth01 --reply-urls https://token.botframework.com/.auth/web/redirect --required-resource-accesses @infrastructure\azuread-manifest.json --query 'appId' -o tsv) 
        * az ad sp credential reset --name $botAuthId --password $botAuthPass --credential-description "default" --years 2

## ARM Template 
    * cd infrastructure 
    * New-AzResourceGroup -Name BOT_RG -Location southcentralus
    * New-AzResourceGroupDeployment -Name bot -ResourceGroupName BOT_RG -TemplateParameterFile .\azuredeploy.parameters.json -TemplateFile .\azuredeploy.json -botApplicationId $botAppId -botApplicationSecret (ConvertTo-SecureString -String $botPass -AsPlainText -Force) -Verbose

## Post Template Configuration
    * Bot Service (https://portal.azure.com/#blade/HubsExtension/BrowseResourceBlade/resourceType/Microsoft.BotService%2FbotServices)
        * Navigate to your bot's Bot Channels Registration page on the Azure Portal.
        * Click Settings.
        * Under OAuth Connection Settings near the bottom of the page, click Add Setting.
        * Fill in the form as follows:
            * Name: AzureAD
            * Service Provider: Azure Active Directory v2
            * Client id: $botAuthId
            * Client Secret: $botAuthPass
            * Tenant: $Your_AzureAD_Tenant_ID
            * Token Exchange URL: Leave Blank
            * For Scopes: openid profile User.Read
        * Test Connection 
        * Channels > Web Chat > Copy WebChannelSecret
        * az webapp config appsettings set -n {{AzureAppServiceName}} -g BOT_RG --settings WebChannelSecret={{WebChannelSecret}}
    * Luis (https://luis.ai)
        * Select the Subscription and the correct Authoring LUIS application
        * Import application 
        * Select Models\FlightBooking.json
        * Train 
        * Publish > Production 
        * Copy Application ID from the Luis Portal under Manage
        * az webapp config appsettings set -n {{AzureAppServiceName}} -g BOT_RG --settings LuisAppId={{LuidAppId}}
    
# Deploy
    * Use Azure DevOps Pipeline with deploy\azure-pipeline.yaml
        *Update Variables for Service Connection and Azure App Service Name 
    * Command Line
        * Update wwwroot\index.html with proper Bot Name
        * cd src
        * dotnet build
        * dotnet publish -o publish 
        * Compress-Archive -Path .\publish\* -DestinationPath bot.zip
        * az webapp deployment source config-zip --resource-group BOT_RG --name bjdbot010-api --src .\bot.zip

