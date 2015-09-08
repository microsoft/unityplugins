@ECHO OFF

for %%X in (unity.exe) do (set FOUND=%%~$PATH:X)
if defined FOUND (echo "Found Unity in path, using it") ELSE GOTO UNITY_PATH_NOT_SET


rem "%UNITY%" == "" GOTO UNITY_PATH_NOT_SET
rem echo Unity.exe -batchmode -projectPath %~dp0Samples\AzureMobileServices -exportPackage Assets  %~dp0UnityPackages\AzureMobileServices.unityPackage -quit  
rem pause

rem Locate where msbuild exists in the system
set bb.build.msbuild.exe=
for /D %%D in (%SYSTEMROOT%\Microsoft.NET\Framework\v4*) do set msbuild.exe=%%D\MSBuild.exe


for %%X in (msbuild.exe) do (set FOUND=%%~$PATH:X)
if defined FOUND (echo "Found msbuild in path, using it") ELSE GOTO MSBUILD_NOT_SET

rem AnyCPU - RELEASE
rem AnyCPU does not work for Advertising because the SDK itself is architecture specific
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release

rem AnyCPU - DEBUG
rem AnyCPU does not work for Advertising
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug

rem x86 - RELEASE
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release /p:Platform="x86"

rem x86 - DEBUG
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug /p:Platform="x86"

rem x64 - RELEASE
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release /p:Platform="x64"

rem x64 - DEBUG
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug /p:Platform="x64"

rem ARM - RELEASE
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Release /p:Platform="arm"

rem ARM - DEBUG
msbuild MainProjects\Win10\Microsoft.UnityPlugins.sln /p:Configuration=Debug /p:Platform="arm"

rem build the editor projects
msbuild EditorProjects\Microsoft.UnityPlugins.sln /p:Configuration=Release

rem copy over the newly generated DLLs to the samples
rem AzureMobileServices
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.AzureMobileServices\Release\*.pdb Samples\AzureMobileServices\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.AzureMobileServices\Release\*.dll Samples\AzureMobileServices\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.AzureMobileServices\Release\AnyCPU\*.pdb Samples\AzureMobileServices\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.AzureMobileServices\Release\AnyCPU\*.dll Samples\AzureMobileServices\Assets\Plugins\WSA

rem Store
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Store\Release\*.pdb Samples\StoreTest\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Store\Release\*.dll Samples\StoreTest\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Store\Release\AnyCPU\*.pdb Samples\StoreTest\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.Store\Release\AnyCPU\*.dll Samples\StoreTest\Assets\Plugins\WSA

rem Core
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.pdb Samples\CoreTest\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.dll Samples\CoreTest\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.pdb Samples\CoreTest\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.dll Samples\CoreTest\Assets\Plugins\WSA

rem Cortana (keeping this separate for now - lots of Cortana specific assets, will later merge with Core)
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.pdb Samples\Cortana\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Core\Release\*.dll Samples\Cortana\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.pdb Samples\Cortana\Assets\Plugins\WSA
xcopy /Y Binaries\Microsoft.UnityPlugins.Core\Release\AnyCPU\*.dll Samples\Cortana\Assets\Plugins\WSA

rem Advertising
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.pdb Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Editor\Microsoft.UnityPlugins.Advertising\Release\*.dll Samples\Advertising\Assets\Plugins
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pdb Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.dll Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.winmd Samples\Advertising\Assets\Plugins\WSA\x86
xcopy /Y Binaries\Microsoft.UnityPlugins.Advertising\Release\x86\*.pri Samples\Advertising\Assets\Plugins\WSA\x86

rem export unitypackages
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

rem echo Finished exporting Unity Package

GOTO END

:MSBUILD_NOT_SET
echo MSBUILD environment variable is not set. Please set it to the msbuild.exe on your system for .NET 4.0 or greater
GOTO END

:UNITY_PATH_NOT_SET
echo UNITY.exe not found in the path. Please include it in the path to build UnityPackages.
GOTO END

:END