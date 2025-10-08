# Fetch the version from EscapeFromTarkov.exe
$tarkovPath = '{0}\..\..\..\EscapeFromTarkov.exe' -f $PSScriptRoot
$tarkovVersion = (Get-Item -Path $tarkovPath).VersionInfo.FileVersionRaw.Revision
Write-Host "Current version of EscapeFromTarkov.exe is: $tarkovVersion"

# Update AssemblyVersion
$versionPath = '{0}\TarkovVersion.cs' -f $PSScriptRoot
$versionPattern = '^\[assembly: TarkovVersion\(.*\)\]'
(Get-Content $versionPath) | ForEach-Object {
    if ($_ -match $versionPattern){
    	$versionType = $matches[1]
        $newLine = '[assembly: TarkovVersion({0})]' -f $tarkovVersion
        Write-Host "Changed the line from '$_' to '$newLine'"
        $newLine
    } else {
        $_
    }
} | Set-Content $versionPath

Write-Host "TarkovVersion.cs updated successfully!"