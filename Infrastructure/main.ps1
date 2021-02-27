[CmdletBinding()]
Param
(
  [Parameter(Mandatory = $false)]
  [string]
  $ApplicationName = "note06",

  [Parameter(Mandatory = $false)]
  [string]
  [ValidateSet('d', 't', 'a', 'p')]
  $Environment = 'd',

  [Parameter(Mandatory = $false)]
  [string]
  [ValidateSet('northeurope', 'westeurope')]
  $Location = "northeurope"
)

Write-Information -MessageData "INFO --- Start script at $(Get-Date -Format 'dd-MM-yyyy HH:mm')." -InformationAction Continue

$resourceGroupName = $($ApplicationName + "-" + $Environment + "-rg").ToLower()
$resourceGroup = Get-AzResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue

if ($resourceGroup) {
  Write-Information "INFO --- Resource group $resourceGroupName already exists." -InformationAction Continue
}
else {
  Write-Information "INFO --- Check succeeded. Resource Group $resourceGroupName doesn't exist." -InformationAction Continue

  # Create the Resource Group with the location and tags.
  $resourceGroup = New-AzResourceGroup -Name $resourceGroupName -Location $Location

  Write-Information "INFO --- Creating resource group with name $resourceGroupName in $Location $($resourceGroup.ProvisioningState)" -InformationAction Continue

  $rgTags = @{administrator = 'adminuser@company.com' }
  $rgTags += @{applicationName = 'notejam' }
  $rgTags += @{costCenter = 'IT007' }
  Set-AzResourceGroup -Name $resourceGroupName -Tag $rgTags | Out-Null
}

$locationAlias = switch ($resourceGroup.Location) {
  'northeurope' { 'ne' }
  'westeurope' { 'we' }
}

$suffix = "01"
Write-Information $PSScriptRoot -InformationAction Continue
Write-Information (Join-Path -Path $PSScriptRoot -ChildPath '\SQLServer\sqlserver.json') -InformationAction Continue
Write-Information (Join-Path -Path $PSScriptRoot -ChildPath '\Web App\webapp.json') -InformationAction Continue
Write-Information (Join-Path -Path $PSScriptRoot -ChildPath '\Frontdoor\frontdoor.json') -InformationAction Continue

try {
  Write-Information "INFO --- Create a Azure Sql." -InformationAction Continue
  $serverName = ($ApplicationName + $locationAlias + $Environment + 'sql' + $suffix).ToLower()
  $serverAdminLogin = $ServerName.Replace('-', '') + 'admin'

  # Generate server admin login password and store in Key Vault
  Add-Type -Assembly System.Web
  $serverAdminLoginPassword = [System.Web.Security.Membership]::GeneratePassword(20, 8)
  $secureServerAdminLoginPassword = ConvertTo-SecureString -String $serverAdminLoginPassword -AsPlainText -Force

  $result = New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName `
    -ErrorAction 'Stop' `
    -Name ($ApplicationName + "_sql_" + (Get-Date).ToString("yyyyMMdd_HHmmss")) `
    -Mode Incremental `
    -TemplateFile (Join-Path -Path $PSScriptRoot -ChildPath "\SQLServer\sqlserver.json") `
    -databaseName $ApplicationName `
    -serverAdminLogin $serverAdminLogin `
    -serverAdminLoginPassword $secureServerAdminLoginPassword `
    -serverName $serverName `
    -Verbose
    # -TemplateFile ".\Infrastructure\SQLServer\sqlserver.json"

}
catch {
  Write-Information -MessageData "INFO --- Sql Server deployment failed." -InformationAction Continue
  throw
}

try {
  Write-Information "INFO --- Create a web app." -InformationAction Continue

  $primarySiteName = "$ApplicationName-public-$Environment-ne-appservice$suffix"
  $secondarySiteName = "$ApplicationName-public-$Environment-we-appservice$suffix"

  $primarySite = New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName `
    -ErrorAction 'Stop' `
    -Name ($ApplicationName + "_webapp_ne_" + (Get-Date).ToString("yyyyMMdd_HHmmss")) `
    -Mode Incremental `
    -TemplateFile (Join-Path -Path $PSScriptRoot -ChildPath "\Web App\webapp.json") `
    -aadAppClientSecret $secureServerAdminLoginPassword `
    -appServicePlanName "$ApplicationName-$Environment-ne-appplan$suffix" `
    -applicationInsightsName "$ApplicationName-$Environment-appinsights$suffix" `
    -appServiceName $primarySiteName `
    -Verbose
    #-TemplateFile ".\Infrastructure\Web App\webapp.json" `

  $secondarySite = New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName `
    -ErrorAction 'Stop' `
    -Name ($ApplicationName + "_webapp_we_" + (Get-Date).ToString("yyyyMMdd_HHmmss")) `
    -Mode Incremental `
    -TemplateFile (Join-Path -Path $PSScriptRoot -ChildPath "\Web App\webapp.json") `
    -aadAppClientSecret $secureServerAdminLoginPassword `
    -appServicePlanName "$ApplicationName-$Environment-we-appplan$suffix" `
    -applicationInsightsName "$ApplicationName-$Environment-we-appinsights$suffix" `
    -appServiceName $secondarySiteName `
    -location 'westeurope' `
    -Verbose

}
catch {
  Write-Information -MessageData "INFO --- Web App deployment failed." -InformationAction Continue
  throw
}

try {
  Write-Information "INFO --- Create a front door." -InformationAction Continue

  $result = New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName `
    -ErrorAction 'Stop' `
    -Name ($ApplicationName + "_frontdoor_" + (Get-Date).ToString("yyyyMMdd_HHmmss")) `
    -Mode Incremental `
    -TemplateFile (Join-Path -Path $PSScriptRoot -ChildPath "\Frontdoor\frontdoor.json") `
    -backendpoolAddress1 "$primarySiteName.azurewebsites.net" `
    -backendpoolAddress2 "$secondarySiteName.azurewebsites.net" `
    -frontDoorName "$ApplicationName-global-frntdoor$suffix" `
    -wafRPolicyName "wafPolicy$suffix" `
    -Verbose
    #-TemplateFile ".\Infrastructure\Frontdoor\frontdoor.json" `
}
catch {
  Write-Information -MessageData "INFO --- App Service deployment failed." -InformationAction Continue
  throw
}


Write-Information -MessageData "INFO --- End script at $(Get-Date -Format 'dd-MM-yyyy HH:mm')." -InformationAction Continue
