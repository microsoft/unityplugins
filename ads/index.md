---
layout: default
title: Advertising Plugin
---

##Introduction
>Before you can use any of the plugins, you will have to register the Unity AppCallbacks with the plugin. This is required so that Windows APIs that require the Windows UI thread can run on it and then call any callbacks back on the Unity thread.
>
>You should place the following line just after *Window.Current.Activate()* in *InitializeUnity* function in App.xaml.cs

```C#
Microsoft.UnityPlugins.Utils.Initialize((action) => AppCallbacks.Instance.InvokeOnAppThread(new AppCallbackItem(() => action()), false));

##APIs

```C#
public class Advertising
{
    public static void Init(string appId, string unitId)
    {
    }

    public static void Show()
    {
    }
}
```

##Samples
A sample is included in the [github repository](https://github.com/Microsoft/unityplugins) under *Samples/Advertising* folder. A Windows Store exported project with the appropriate settings is present in the *Samples/Advertising/out_win10* folder.