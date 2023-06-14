$neosDir = $null
$externalLibDir = '.\ExternalLibraries'
$hasExternalLibrariesDir = Test-Path $externalLibDir


function Get-NeosDir {
	$dir = $env:NeosPath
	if ($dir) { return $dir }

	Write-Host "Type in the directory where Neos.exe resides in..."
	$env:NeosPath = Read-Host
	Write-Host "Setting environment variable 'NeosPath' to input. You can always change this outside of the script!"

	return $env:NeosPath
}

function AddDll($Name, $AdditionalPath = "Neos_Data\Managed\") {
	$dllPath = "$($externalLibDir)\$($Name).dll"
	$hasDll = Test-Path $dllPath

	if (!$hasDll) {
		if (!$neosDir) {
			$neosDir = Get-NeosDir
		}
		New-Item -ItemType SymbolicLink -Path $dllPath -Target "$($neosDir)\$($AdditionalPath)$($Name).dll"
	}

}


if (!$hasExternalLibrariesDir) {
	New-Item $externalLibDir -ItemType Directory | Out-Null
}

AddDll("BaseX")
AddDll "crnlib" 'Neos_Data\Plugins\x86_64\'
AddDll("crunch.NET")
AddDll "FreeImage" 'Neos_Data\Plugins\x86_64\'
AddDll("FreeImageNET")
AddDll("FrooxEngine")
AddDll("CodeX")
AddDll("CloudX.Shared")
AddDll("Octokit")
AddDll("Ben.Demystifier")
AddDll("LZMA")
AddDll("Microsoft.AspNetCore.Connections.Abstractions")
AddDll("Microsoft.AspNetCore.Http.Connections.Client")
AddDll("Microsoft.AspNetCore.Http.Connections.Common")
AddDll("Microsoft.AspNetCore.Http.Features")
AddDll("Microsoft.AspNetCore.SignalR.Client.Core")
AddDll("Microsoft.AspNetCore.SignalR.Client")
AddDll("Microsoft.AspNetCore.SignalR.Common")
AddDll("Microsoft.AspNetCore.SignalR.Protocols.Json")
AddDll("Microsoft.Extensions.DependencyInjection")
AddDll("Microsoft.Extensions.DependencyInjection.Abstractions")
AddDll("Microsoft.Extensions.Logging")
AddDll("Microsoft.Extensions.Logging.Abstractions")
AddDll("Microsoft.Extensions.Options")
AddDll("Microsoft.Extensions.Primitives")
AddDll("SignalR.Strong")
AddDll("System.IO.Pipelines")