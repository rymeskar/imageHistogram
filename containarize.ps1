param (
  [switch] $Build = $True,
  [switch] $Push = $False,
  [switch] $RestartService = $False,
  [switch] $CreateContainer = $False
)

if ($Build)
{
    docker build -t image_histogram ImageHistogram/.
    docker tag image_histogram karymes.azurecr.io/image_histogram
}

if ($Push)
{
    az acr login --name karymes.azurecr.io/image_histogram
    docker push karymes.azurecr.io/image_histogram
}

if ($RestartService)
{
    az container restart --name imagehistogram --resource-group imagehistogram
    Invoke-WebRequest http://imagehistogram.northeurope.azurecontainer.io
}

if ($CreateContainer)
{
	$ACI_PERS_RESOURCE_GROUP = "afp"
	$ACI_PERS_STORAGE_ACCOUNT_NAME = "apicategorizer"
	$STORAGE_KEY=$(az storage account keys list --resource-group $ACI_PERS_RESOURCE_GROUP --account-name $ACI_PERS_STORAGE_ACCOUNT_NAME --query "[0].value" --output tsv)
	az acr login --name karymes.azurecr.io/image_histogram
	az container create `
		--resource-group imagehistogram `
		--name imagehistogram `
		--image karymes.azurecr.io/image_histogram `
		--dns-name-label imagehistogram `
		--ports 80 `
		--azure-file-volume-account-name $ACI_PERS_STORAGE_ACCOUNT_NAME `
		--azure-file-volume-account-key $STORAGE_KEY `
		--azure-file-volume-share-name image-histogram `
		--azure-file-volume-mount-path /images
}
