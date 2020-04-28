param (
    [string] $ResourceGroupName,
    [string] $location,
    [string] $functionAppName
)

funcStorageName="{0}sa" -f $functionAppName

az storage account create --name $funcStorageName --location $location --resource-group $ResourceGroupName --sku Standard_LRS
az functionapp create --name $functionAppName --storage-account $funcStorageName --consumption-plan-location $location --resource-group $ResourceGroupName
