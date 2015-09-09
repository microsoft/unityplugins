# Unity plugins for Windows Platforms.
The structure will be fairly simple. At the time, the intent is to have 5 Unity plugins:



Documentation for the APIs is present at: http://microsoft.github.io/unityplugins

*Following documentation only tells you how to build the packages*

##Store
Contains licensing functionality

##Core
Contains utilities, tiles, toasts, badges etc.

##AzureMobile
Contains CRUD operations for Azure Mobile Services

##Ads
Contains Ad APIs for Microsoft Ads

##Social (In progress)
Mostly a wrapper for Facebook 


#Build Instructions
* We use powershell to build all the projects and export UnityPackages. Make sure you set the variables in the buildall.ps1 script
* IMPORTANT: Make sure that you have set the path to the msbuild included with Visual Studio 
   as the $msbuild_vs variable in the buildall.ps1 script. Plugins do not build with .net msbuild
* Visual Studio 2015 is REQUIRED to build these plugins
* Plugins only work on Windows 10 at the moment
* Make sure that the $unity_exe variable is set to the correct path for Unity in the buildall.ps1 script
* Run the following in your powershell to allow build script. Answer 'Y' to the question on allowing 
   unsigned scripts to run.
		'Set-ExecutionPolicy -Scope CurrentUser unrestricted'
* Install Windows Azure SDK for Visual Studio 2015 from http://azure.microsoft.com/en-us/downloads/ 
* Install the latest Microsoft Advertising SDK from http://adsinapps.microsoft.com/en-us/
* Make sure you install the latest version of nuget  from http://blog.nuget.org/20150902/nuget-3.2RC.html (3.2+) and set its path in the $nuget_exe variable in the buildall.ps1 script
 * run buildall.ps1 script in the root folder. This will build all the plugins in the various flavors Debug/Release/architecture and place the binaries in the "Binaries" folder in the root of the project. It will additionally also copy the newly built DLLs to the Samples and run Unity to export the packages into the UnityPackages folder in the project root.
 * You DO NOT need to use the Cortana package. All Cortana functionality is present in the Core plugin and that is sufficient. Use the Cortana unity package as a learning sample.
 * The script is set to stop on any failure. The last failure you see is the one that broke the build.
 
 
 