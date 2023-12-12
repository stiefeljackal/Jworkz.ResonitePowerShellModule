$resoniteDir = $null
$externalLibDir = '.\ExternalLibraries'
$hasExternalLibrariesDir = Test-Path $externalLibDir


function Get-ResoniteDir {
	$dir = $env:ResonitePath
	if ($dir) { return $dir }

	Write-Host "Type in the directory where Resonite.exe resides in..."
	$env:ResonitePath = Read-Host
	Write-Host "Setting environment variable 'ResonitePath' to input. You can always change this outside of the script!"

	return $env:ResonitePath
}

function AddDll($Name, $AdditionalPath = "Resonite_Data\Managed\") {
	$dllPath = "$($externalLibDir)\$($Name).dll"
	$hasDll = Test-Path $dllPath

	if (!$hasDll) {
		if (!$resoniteDir) {
			$resoniteDir = Get-ResoniteDir
		}
		New-Item -ItemType SymbolicLink -Path $dllPath -Target "$($resoniteDir)\$($AdditionalPath)$($Name).dll"
	}

}


if (!$hasExternalLibrariesDir) {
	New-Item $externalLibDir -ItemType Directory | Out-Null
}

AddDll "Elements.Core"
AddDll "Elements.Assets"
AddDll "Skyfrost.Base"
AddDll "Hardware.Info"
AddDll "SignalR.Strong"
AddDll "crnlib" 'Resonite_Data\Plugins\x86_64\'
AddDll "FreeImage" 'Resonite_Data\Plugins\x86_64\'