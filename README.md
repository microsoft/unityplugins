# Unity plugins for Windows Platforms.
The structure will be fairly simple. At the time, the intent is to have 5 Unity plugins:

##Store
Contains licensing functionality

##Core
Contains utilities, tiles, toasts, badges etc.

##AzureMobile
Contains CRUD operations for Azure Mobile Services

##Ads
Contains Ad APIs for Microsoft Ads

##Social
Mostly a wrapper for Facebook 


#Build Instructions
 * Install Windows Azure SDK for Visual Studio 2015 from http://azure.microsoft.com/en-us/downloads/ 
 * Install the latest Microsoft Advertising SDK from http://adsinapps.microsoft.com/en-us/
 * [Working on making command line build possible using auto nuget store] In the meanwhile, make sure to open the UnityPlugins.sln in Win10 
 folder and build once manually to make sure that all releveant packages etc have been restored and built once.
 * Open a Developer Command for Visual Studio 2015 by pressing the Windows button and typing "Develper". This step is required to setup MSBuild in your path in the correct manner
 * Ensure that unity.exe is in your path. Usually, the path looks like "C:\program files\unity\editor"
 * run buildall.bat script in the root folder. This will build all the plugins in the various flavors Debug/Release/architecture and place the binaries in the "Binaries" folder in the root of the project. It will additionally also copy the newly built DLLs to the Samples and run Unity to export the packages into the UnityPackages folder in the project root.
 * You DO NOT need to use the Cortana package. All Cortana functionality is present in the Core plugin and that is sufficient. Use the Cortana unity package as a learning sample.
 
 
 