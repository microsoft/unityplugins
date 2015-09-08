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

function exitIfFailed
{
	if($1 -ne 0)
	{
		write-host '=========================================' -foreground 'red'
		write-host 'Failed building all projects. Exiting now' -foreground 'red'
		write-host '=========================================' -foreground 'red'
		Exit
	}
	else
	{
		write-host '=========================================' -foreground 'green'
		write-host 'Successfully completed build step' -foreground 'green'
		write-host '=========================================' -foreground 'green'
	}
}




$msbuild_vs = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$unity_exe = "C:\Program Files\Unity\Editor\unity.exe"

& $msbuild_vs MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release
exitIfFailed

# AnyCPU - RELEASE
# AnyCPU does not work for Advertising because the SDK itself is architecture specific
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release
exitIfFailed

# AnyCPU - DEBUG
# AnyCPU does not work for Advertising
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug
exitIfFailed

# x86 - RELEASE
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release /p:Platform="x86"
exitIfFailed

# x86 - DEBUG
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug /p:Platform="x86"
exitIfFailed

# x64 - RELEASE
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release /p:Platform="x64"
exitIfFailed

# x64 - DEBUG
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug /p:Platform="x64"
exitIfFailed

# ARM - RELEASE
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release /p:Platform="arm"
exitIfFailed

# ARM - DEBUG
& $msbuild_vs  MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug /p:Platform="arm"
exitIfFailed

# build the editor projects
& $msbuild_vs  EditorProjects\Microsoft.UnityPlugins.sln /p:Configuration=Release
exitIfFailed

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

# Advertising
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pdb Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.dll Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.winmd Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pri Samples\Advertising\Assets\Plugins\WSA\x86

# export unitypackages
echo "Exporting Unity packages.. Waiting till unity is done."

echo "Exporting Azure Mobile Services package"
Unity.exe -batchmode -projectPath %~dp0Samples\AzureMobileServices -exportPackage Assets  %~dp0UnityPackages\Microsoft.UnityPlugins.AzureMobileServices.unityPackage -quit  
echo "[Success] Exporting Azure Mobile Services package"


echo "Exporting Advertising package"
Unity.exe -batchmode -projectPath %~dp0Samples\Advertising -exportPackage Assets  %~dp0UnityPackages\Microsoft.UnityPlugins.Advertising.unityPackage -quit  
echo "[Success] Exporting Advertising package"

echo "Exporting Core package"
Unity.exe -batchmode -projectPath %~dp0Samples\CoreTest -exportPackage Assets  %~dp0UnityPackages\Microsoft.UnityPlugins.Core.unityPackage -quit  
echo "[Success] Exporting Core package"

echo "Exporting Store package"
Unity.exe -batchmode -projectPath %~dp0Samples\StoreTest -exportPackage Assets  %~dp0UnityPackages\Microsoft.UnityPlugins.Store.unityPackage -quit
echo "[Success] Exporting Store package"

echo "Exporting Cortana package"
Unity.exe -batchmode -projectPath %~dp0Samples\Cortana -exportPackage Assets  %~dp0UnityPackages\Microsoft.UnityPlugins.Cortana.unityPackage -quit
echo "[Success] Exporting Cortana package"

echo "Exporting Unity packages done. Packages should be in UnityPackages folder."

# echo Finished exporting Unity Package