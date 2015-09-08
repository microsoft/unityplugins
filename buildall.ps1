<#
1. IMPORTANT: Make sure that you have set the path to the msbuild included with Visual Studio 
as the $msbuild_vs variable below. Plugins do not build with .net msbuild
2. Make sure that the  unity_exe variable is set to the correct path for Unity
3. Run the following in your powershell to allow build script. Answer 'Y' to the question on allowing 
   unsigned scripts to run.
		'Set-ExecutionPolicy -Scope CurrentUser unrestricted'
#>

#Set-ExecutionPolicy -Scope CurrentUser unrestricted
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

function WriteMessage($message, $color)
{
	write-host $message -foreground $color
}

$msbuild_vs = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$unity_exe = "C:\Program Files\Unity\Editor\unity.exe"

# AnyCPU - RELEASE
# AnyCPU does not work for Advertising because the SDK itself is architecture specific
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release
exitIfFailed 'Store, Core, Azure - AnyCPU, RELEASE'

# AnyCPU - DEBUG
# AnyCPU does not work for Advertising
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug
exitIfFailed 'Store, Core, Azure - AnyCPU, DEBUG'

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

# build the editor projects
& $msbuild_vs  EditorProjects\Microsoft.UnityPlugins.sln /p:Configuration=Release
exitIfFailed 'Editor Projects - AnyCPU, RELEASE'

WriteMessage "==========================================" "magenta"
WriteMessage "begin copying DLLs to Unity Samples" "magenta"
WriteMessage "==========================================" "magenta"

# copy over the newly generated DLLs to the samples
# AzureMobileServices
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.AzureMobileServices\Release\*.pdb Samples\AzureMobileServices\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.AzureMobileServices\Release\*.dll Samples\AzureMobileServices\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.AzureMobileServices\Release\AnyCPU\*.pdb Samples\AzureMobileServices\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.AzureMobileServices\Release\AnyCPU\*.dll Samples\AzureMobileServices\Assets\Plugins\WSA

# Store
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Store\Release\*.pdb Samples\StoreTest\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Store\Release\*.dll Samples\StoreTest\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Store\Release\AnyCPU\*.pdb Samples\StoreTest\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.Store\Release\AnyCPU\*.dll Samples\StoreTest\Assets\Plugins\WSA

# Core
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.pdb Samples\CoreTest\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.dll Samples\CoreTest\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.pdb Samples\CoreTest\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.dll Samples\CoreTest\Assets\Plugins\WSA

# Cortana (keeping this separate for now - lots of Cortana specific assets, will later merge with Core)
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.pdb Samples\Cortana\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.dll Samples\Cortana\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.pdb Samples\Cortana\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.dll Samples\Cortana\Assets\Plugins\WSA

#create the x86,x64 and ARM folders if they don't exist. Advertising SDK is architecture specific
New-Item -ItemType Directory -Force -Path Samples\Advertising\Assets\Plugins\WSA\x86
New-Item -ItemType Directory -Force -Path Samples\Advertising\Assets\Plugins\WSA\x64
New-Item -ItemType Directory -Force -Path Samples\Advertising\Assets\Plugins\WSA\ARM

# Advertising x86 files
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pdb Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.dll Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.winmd Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pri Samples\Advertising\Assets\Plugins\WSA\x86

# Advertising x64 files
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.pdb Samples\Advertising\Assets\Plugins\WSA\x64
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.dll Samples\Advertising\Assets\Plugins\WSA\x64
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.winmd Samples\Advertising\Assets\Plugins\WSA\x64
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x64\*.pri Samples\Advertising\Assets\Plugins\WSA\x64

# Advertising ARM files
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.pdb Samples\Advertising\Assets\Plugins\WSA\ARM
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.dll Samples\Advertising\Assets\Plugins\WSA\ARM
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.winmd Samples\Advertising\Assets\Plugins\WSA\ARM
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\ARM\*.pri Samples\Advertising\Assets\Plugins\WSA\ARM

WriteMessage "==========================================" "green"
WriteMessage "Successfully Copied all binaries to Unity Samples" "green"
WriteMessage "==========================================" "green"

WriteMessage "==========================================" "magenta"
WriteMessage "Begin exporting unity packages" "magenta"
WriteMessage "==========================================" "magenta"

WriteMessage "Exporting Unity packages.. Waiting till unity is done." "magenta"

$currentPath = convert-path .
WriteMessage "Exporting Azure Mobile Services package" "magenta"

& $unity_exe  -batchmode -projectPath $currentPath\Samples\AzureMobileServices -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.AzureMobileServices.unityPackage -quit  | Out-Null
exitIfFailed "Exporting Azure Mobile Services package"

WriteMessage "Exporting Advertising package" "magenta"
& $unity_exe  -batchmode -projectPath $currentPath\Samples\Advertising -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Advertising.unityPackage -quit  | Out-Null
exitIfFailed "Exporting Advertising package"

WriteMessage "Exporting Core package" "magenta"
& $unity_exe  -batchmode -projectPath $currentPath\Samples\CoreTest -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Core.unityPackage -quit  | Out-Null
exitIfFailed "Exporting Core package" 

WriteMessage "Exporting Store package" "magenta"
& $unity_exe  -batchmode -projectPath $currentPath\Samples\StoreTest -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Store.unityPackage -quit | Out-Null
exitIfFailed "Exporting Store package"

WriteMessage "Exporting Cortana package" "magenta"
& $unity_exe  -batchmode -projectPath $currentPath\Samples\Cortana -exportPackage Assets  $currentPath\UnityPackages\Microsoft.UnityPlugins.Cortana.unityPackage -quit | Out-Null
exitIfFailed "Exporting Cortana package"

WriteMessage "Exporting Unity packages done. Packages should be in UnityPackages folder." "green"

WriteMessage "************************************************************" "green"
WriteMessage "            All steps completed successfully. Enjoy!        " "green"
WriteMessage "************************************************************" "green"