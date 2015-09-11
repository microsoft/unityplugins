---
layout: default
title: Azure Plugin
---

##Introduction

>Before you can use any of the plugins, you will have to register the Unity AppCallbacks with the plugin. This is required so that Windows APIs that require the Windows UI thread can run on it and then call any callbacks back on the Unity thread.
>
>You should place the following line just after *Window.Current.Activate()* in *InitializeUnity* function in App.xaml.cs

```C#
Microsoft.UnityPlugins.Utils.Initialize((action) => AppCallbacks.Instance.InvokeOnAppThread(new AppCallbackItem(() => action()), false));
```

##APIs

###Enumerations
```C#
public enum MobileServiceAuthenticationProvider
{
    Facebook = 0,
    Google = 1,
    MicrosoftAccount = 2,
    Twitter = 3,
    WindowsAzureDirectory = 4
}
```	
###Proxy/stub classes

```C#
public class MobileServiceUser
{
    public string MobileServiceAuthenticationToken { get; set; }
    public string UserId { get; set; }
}
```
	
###AzureMobileServices classes

```C#
public class AzureMobileServices
{
    public static void AuthenticateWithServiceProvider(MobileServiceAuthenticationProvider authenticationProvider, 
        Action<MobileServiceUser> OnAuthenticationFinished);
    public static void Connect(string applicationUri, string applicationKey);
    public static void Insert<T>(T item, Action OnInsertFinished);
    public static void Lookup<T>(string id, Action<T> OnLookupFinished);
    public static void Update<T>(T item, Action OnUpdateFinished);
    public static void Delete<T>(T item, Action OnDeleteFinished);
    public static void Where<T>(Expression<Func<T, bool>> predicate, Action<List<T>> OnWhereFinished);
}
```
##Samples
A sample is included in the [github repository](https://github.com/Microsoft/unityplugins) under *Samples/AzureMobileServices* folder. A Windows Store exported project with the appropriate settings is present in the *Samples/AzureMobileServices/out_win10* folder.