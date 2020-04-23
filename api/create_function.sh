#!/bin/bash

export RG=$1
export location=$2
export functionAppName=$3

az login 
az group create -n $RG -l $location

funcStorageName=${functionAppName}sa

az storage account create --name $funcStorageName --location $location --resource-group $RG --sku Standard_LRS
az functionapp create --name $functionAppName --storage-account $funcStorageName --consumption-plan-location $location --resource-group $RG
