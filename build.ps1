<#
- IMPORTANT: Make sure that you have set the path to the msbuild included with Visual Studio 
   as the $msbuild_vs variable below. Plugins do not build with .net msbuild
- Visual Studio 2015 is REQUIRED
- Plugins only work on Windows 10 at the moment
- Make sure that the $unity_exe variable is set to the correct path for Unity
- Run the following in your powershell to allow build script. Answer 'Y' to the question on allowing 
   unsigned scripts to run.
		'Set-ExecutionPolicy -Scope CurrentUser unrestricted'
- Install Windows Azure SDK for Visual Studio 2015 from http://azure.microsoft.com/en-us/downloads/ 
- Install the latest Microsoft Advertising SDK from http://adsinapps.microsoft.com/en-us/
- Make sure you install the latest version of nuget from and set it in the $nuget_exe variable below
#>

Param(
	[Switch]$store,
	[Switch]$core,
	[Switch]$azure,
	[Switch]$ads,
	[Switch]$all,
	[Switch]$help
)

function WriteMessage($message, $color)
{
	write-host $message -foreground $color
}

$helpMessage = 
@"
Usage: build.ps1 [-store] [-core] [-ads] [-azure] [-all]
Options:
	-store: builds and exports store package
	-core: builds and exports core package
	-ads: builds and exports Advertising package
	-azure: builds and exports azure package
	-all: builds and exports all the packages
	-help: prints this message
"@

if($help)
{
	WriteMessage $helpMessage 'green'
	Exit
}

if($store -eq $false -and $ads -eq $false -and $core -eq $false -and $azure -eq $false -and $all -eq $false)
{
	WriteMessage $helpMessage 'green'
	Exit
}

write-host "`n=================================="
write-host "Starting building the SDK"
write-host "==================================`n"

function exitIfFailed($message)
{
	if($lastexitcode -ne 0)
	{
		write-host '=============================================================' -foreground 'red'
		write-host '[Failed]' $message 'project. Exiting now' -foreground 'red'
		write-host ============================================================= -foreground 'red'
		Exit
	}
	else
	{
		write-host '=============================================================' -foreground 'green'
		write-host '[Success]' $message -foreground 'green'
		write-host '=============================================================' -foreground 'green'
	}
}





# IMPORTANT: MAKE SURE TO SET THESE VARIABLES TO YOUR ENVIRONMENT TO AVOID
# BUILD FAILURES
$msbuild_vs = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$unity_exe = "C:\Program Files\Unity\Editor\unity.exe"
$nuget_exe = "c:\tools\nuget.exe"


WriteMessage "Build Editor Projects" "magenta"

& $msbuild_vs  EditorProjects\Microsoft.UnityPlugins.sln /p:Configuration=Release
exitIfFailed 'Editor projects - AnyCPU, RELEASE'

& $msbuild_vs  EditorProjects\Microsoft.UnityPlugins.sln /p:Configuration=Debug
exitIfFailed 'Editor projects - AnyCPU, Debug'
WriteMessage "Finished Building Editor Projects" "green"

if($store -or $all)
{
	WriteMessage "Store specified on the command line, building store now" "magenta"
	& $nuget_exe restore MainProjects\Win10\Microsoft.UnityPlugins.Store.sln
	exitIfFailed 'Nuget restore Store'

	# AnyCPU - RELEASE
	# AnyCPU does not work for Advertising because the SDK itself is architecture specific
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Store.sln /p:Configuration=Release
	exitIfFailed 'Store - AnyCPU, RELEASE'

	# AnyCPU - DEBUG
	# AnyCPU does not work for Advertising
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Store.sln /p:Configuration=Debug
	exitIfFailed 'Store - AnyCPU, DEBUG'


	WriteMessage "==========================================" "magenta"
	WriteMessage "begin copying DLLs to Unity Sample" "magenta"
	WriteMessage "==========================================" "magenta"

	# Copy Store binaries to samples
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Store\Release\*.pdb Samples\StoreTest\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Store\Release\*.dll Samples\StoreTest\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Store\Release\AnyCPU\*.pdb Samples\StoreTest\Assets\Plugins\WSA
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Store\Release\AnyCPU\*.dll Samples\StoreTest\Assets\Plugins\WSA
	exitIfFailed "xcopying file(s)"

	WriteMessage "==========================================" "green"
	WriteMessage "Successfully Copied all binaries to Unity Samples" "green"
	WriteMessage "==========================================" "green"

	WriteMessage "==========================================" "magenta"
	WriteMessage "Begin exporting unity packages" "magenta"
	WriteMessage "==========================================" "magenta"

	$currentPath = convert-path .
	WriteMessage "Exporting Azure Mobile Services package" "magenta"

	WriteMessage "Exporting Store package" "magenta"
	& $unity_exe  -batchmode -projectPath $currentPath\Samples\StoreTest -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Store.unityPackage -quit | Out-Null
	exitIfFailed "Exporting Store package"

	WriteMessage "Exporting Store UnityPackage done. Package should be in UnityPackages folder." "green"
}

if($core -or $all)
{
	WriteMessage "Core specified on the command line, building Core now" "magenta"
	& $nuget_exe restore MainProjects\Win10\Microsoft.UnityPlugins.Core.sln
	exitIfFailed 'Nuget restore Core'

	# AnyCPU - RELEASE
	# AnyCPU does not work for Advertising because the SDK itself is architecture specific
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Core.sln /p:Configuration=Release
	exitIfFailed 'Core - AnyCPU, RELEASE'

	# AnyCPU - DEBUG
	# AnyCPU does not work for Advertising
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Core.sln /p:Configuration=Debug
	exitIfFailed 'Core - AnyCPU, DEBUG'

	WriteMessage "==========================================" "magenta"
	WriteMessage "begin copying DLLs to Unity Sample" "magenta"
	WriteMessage "==========================================" "magenta"

	# Core
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.pdb Samples\CoreTest\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.dll Samples\CoreTest\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.pdb Samples\CoreTest\Assets\Plugins\WSA
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.dll Samples\CoreTest\Assets\Plugins\WSA
	exitIfFailed "xcopying file(s)"

	WriteMessage "==========================================" "green"
	WriteMessage "Successfully Copied all binaries to Unity Samples" "green"
	WriteMessage "==========================================" "green"

	WriteMessage "==========================================" "magenta"
	WriteMessage "Begin exporting unity packages" "magenta"
	WriteMessage "==========================================" "magenta"

	$currentPath = convert-path .

	WriteMessage "Exporting Core package" "magenta"
	& $unity_exe  -batchmode -projectPath $currentPath\Samples\CoreTest -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Core.unityPackage -quit  | Out-Null
	exitIfFailed "Exporting Core package" 

	WriteMessage "Exporting Cortana package" "magenta"
	& $unity_exe  -batchmode -projectPath $currentPath\Samples\Cortana -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Cortana.unityPackage -quit | Out-Null
	exitIfFailed "Exporting Cortana package"

	WriteMessage "Exporting Core UnityPackage done. Package should be in UnityPackages folder." "green"
}

if($ads -or $all)
{
	WriteMessage "Ads specified on the command line, building Ads now" "magenta"
	& $nuget_exe restore MainProjects\Win10\Microsoft.UnityPlugins.Advertising.sln
	exitIfFailed 'Nuget restore Core'

	# x86 - RELEASE
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Advertising.sln /p:Configuration=Release /p:Platform="x86"
	exitIfFailed 'Advertising - x86, RELEASE'

	# x86 - DEBUG
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Advertising.sln /p:Configuration=Debug /p:Platform="x86"
	exitIfFailed 'Advertising - x86, DEBUG'

	# x64 - RELEASE
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Advertising.sln /p:Configuration=Release /p:Platform="x64"
	exitIfFailed 'Advertising - x64, RELEASE'

	# x64 - DEBUG
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Advertising.sln /p:Configuration=Debug /p:Platform="x64"
	exitIfFailed 'Advertising - x64, DEBUG'

	# ARM - RELEASE
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Advertising.sln /p:Configuration=Release /p:Platform="arm"
	exitIfFailed 'Advertising - ARM, RELEASE'

	# ARM - DEBUG
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.Advertising.sln /p:Configuration=Debug /p:Platform="arm"
	exitIfFailed 'Advertising - ARM, DEBUG'

	WriteMessage "==========================================" "magenta"
	WriteMessage "begin copying DLLs to Unity Sample" "magenta"
	WriteMessage "==========================================" "magenta"

	#create the x86,x64 and ARM folders if they don't exist. Advertising SDK is architecture specific
	New-Item -ItemType Directory -Force -Path Samples\Advertising\Assets\Plugins\WSA\x86
	New-Item -ItemType Directory -Force -Path Samples\Advertising\Assets\Plugins\WSA\x64
	New-Item -ItemType Directory -Force -Path Samples\Advertising\Assets\Plugins\WSA\ARM

	# Advertising x86 files
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pdb Samples\Advertising\Assets\Plugins\WSA\x86
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.dll Samples\Advertising\Assets\Plugins\WSA\x86
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.winmd Samples\Advertising\Assets\Plugins\WSA\x86
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pri Samples\Advertising\Assets\Plugins\WSA\x86
	exitIfFailed "xcopying file(s)"

	WriteMessage "==========================================" "green"
	WriteMessage "Successfully Copied all binaries to Unity Samples" "green"
	WriteMessage "==========================================" "green"

	WriteMessage "==========================================" "magenta"
	WriteMessage "Begin exporting unity packages" "magenta"
	WriteMessage "==========================================" "magenta"

	# Advertising x64 files
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.pdb Samples\Advertising\Assets\Plugins\WSA\x64
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.dll Samples\Advertising\Assets\Plugins\WSA\x64
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.winmd Samples\Advertising\Assets\Plugins\WSA\x64
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.pri Samples\Advertising\Assets\Plugins\WSA\x64
	exitIfFailed "xcopying file(s)"

	# Advertising ARM files
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.pdb Samples\Advertising\Assets\Plugins\WSA\ARM
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.dll Samples\Advertising\Assets\Plugins\WSA\ARM
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.winmd Samples\Advertising\Assets\Plugins\WSA\ARM
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.pri Samples\Advertising\Assets\Plugins\WSA\ARM
	exitIfFailed "xcopying file(s)"

	$currentPath = convert-path .
	WriteMessage "Exporting Advertising package" "magenta"

	& $unity_exe  -batchmode -projectPath $currentPath\Samples\Advertising -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Advertising.unityPackage -quit  | Out-Null
	exitIfFailed "Exporting Advertising package"
 
	WriteMessage "Exporting Advertising UnityPackage done. Package should be in UnityPackages folder." "green"
}

if($azure -or $all)
{
	WriteMessage "Azure specified on the command line, building Azure now" "magenta"
	& $nuget_exe restore MainProjects\Win10\Microsoft.UnityPlugins.AzureMobileServices.sln
	exitIfFailed 'Nuget restore Core'

	# AnyCPU - RELEASE
	# AnyCPU does not work for Advertising because the SDK itself is architecture specific
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.AzureMobileServices.sln /p:Configuration=Release
	exitIfFailed 'AzureMobileServices - AnyCPU, RELEASE'

	# AnyCPU - DEBUG
	# AnyCPU does not work for Advertising
	& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.AzureMobileServices.sln /p:Configuration=Debug
	exitIfFailed 'AzureMobileServices - AnyCPU, DEBUG'

	WriteMessage "==========================================" "magenta"
	WriteMessage "begin copying DLLs to Unity Sample" "magenta"
	WriteMessage "==========================================" "magenta"

	# AzureMobileServices
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.AzureMobileServices\Release\*.pdb Samples\AzureMobileServices\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.AzureMobileServices\Release\*.dll Samples\AzureMobileServices\Assets\Plugins
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.AzureMobileServices\Release\AnyCPU\*.pdb Samples\AzureMobileServices\Assets\Plugins\WSA
	exitIfFailed "xcopying file(s)"
	xcopy /Y Binaries\Microsoft.UnityPlugins.AzureMobileServices\Release\AnyCPU\*.dll Samples\AzureMobileServices\Assets\Plugins\WSA
	exitIfFailed "xcopying file(s)"

	WriteMessage "==========================================" "green"
	WriteMessage "Successfully Copied all binaries to Unity Samples" "green"
	WriteMessage "==========================================" "green"

	WriteMessage "==========================================" "magenta"
	WriteMessage "Begin exporting unity packages" "magenta"
	WriteMessage "==========================================" "magenta"

	$currentPath = convert-path .

	WriteMessage "Exporting AzureMobileServices package" "magenta"
	& $unity_exe  -batchmode -projectPath $currentPath\Samples\AzureMobileServices -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.AzureMobileServices.unityPackage -quit  | Out-Null
	exitIfFailed "Exporting Azure Mobile Services package"

	WriteMessage "Exporting AzureMobileServices UnityPackage done. Package should be in UnityPackages folder." "green"
}

WriteMessage "************************************************************" "green"
WriteMessage "            All steps completed successfully. Enjoy!        " "green"
WriteMessage "************************************************************" "green"