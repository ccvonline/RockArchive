# This script is run by AppVeyor's deploy agent after the deploy
Import-Module WebAdministration
Import-Module IISAdministration


$rootfolder = "c:\webdata\rock.ccv.church"
$webroot = "$rootfolder\docs"

Write-Output "Running post-deploy script"
Write-Output "--------------------------------------------------"
Write-Output "Root folder: $rootfolder"
Write-Output "Web root folder: $webroot"
Write-Output "Running script as: $env:userdomain\$env:username"
 
# ensure that the compilation debug is false
(Get-Content "$webroot\web.Compilation.config").Replace('<compilation debug="true"', '<compilation debug="false"') | Set-Content "$webroot\web.Compilation.config"

# delete the content directory if it exists as it was added by the deploy
If (Test-Path "$webroot\Content"){
	Remove-Item "$webroot\Content" -Force -Confirm:$False -Recurse
}
 
# move content directory back from temp
Write-Host "Moving Content folder back from temp directory"
Move-Item "$rootfolder\temp\Content" "$webroot"

# copy new connection string file
Write-Host "Copying new web.ConnectionStrings.config to web dir"
Copy-Item "$rootfolder\config\web.ConnectionStrings.config" $webroot -force

# Add modify permissions for app pool to site
Write-Host "Adding modify permission for AppPool to site"

# get the IIS App Pool
$manager = Get-IISServerManager
$appPoolName = "RockRMS"
$appPoolSid = $manager.ApplicationPools["$appPoolName"].RawAttributes['applicationPoolSid']
$identifier = New-Object System.Security.Principal.SecurityIdentifier $appPoolSid
$user = $identifier.Translate([System.Security.Principal.NTAccount])

# set permissions
$path = "C:\webdata\rock.ccv.church\docs"
$permission = $user,"Modify","Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission
$acl = Get-ACL $path
$acl.SetAccessRule($accessRule)
$acl | Set-ACL $path

# start web publishing service
Write-Host "Starting Web Publishing Service"
start-service -servicename w3svc

# create empty migration flag
New-Item "$webroot\App_Data\Run.Migration" -type file -force

# set acl on migration flag file so the app has permissions to delete it
$acl = Get-ACL "$webroot\App_Data\Run.Migration"
$accessRule= New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone","FullControl","Allow")
$acl.AddAccessRule($accessRule)
Set-Acl "$webroot\App_Data\Run.Migration" $acl

# delete deploy scripts
If (Test-Path "$webroot\deploy.ps1"){
	Remove-Item "$webroot\deploy.ps1"
}

If (Test-Path "$webroot\before-deploy.ps1"){
	Remove-Item "$webroot\before-deploy.ps1"
}

# delete the appveyor deploy cache
If (Test-Path c:\appveyor){
	Remove-Item c:\appveyor -Force -Confirm:$False -Recurse
}