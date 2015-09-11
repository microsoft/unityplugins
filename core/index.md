---
layout: default
title: Core Plugin
---

##Introduction
>Before you can use any of the plugins, you will have to register the Unity AppCallbacks with the plugin. This is required so that Windows APIs that require the Windows UI thread can run on it and then call any callbacks back on the Unity thread.
>
>You should place the following line just after *Window.Current.Activate()* in *InitializeUnity* function in App.xaml.cs in the exported Windows Universal project.

```C#
Microsoft.UnityPlugins.Utils.Initialize((action) => AppCallbacks.Instance.InvokeOnAppThread(new AppCallbackItem(() => action()), false));
```

##APIs

###Enumerations

An explanation of all the tile types is [here](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.notifications.tiletemplatetype.aspx). The same for toasts is [here](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.notifications.toasttemplatetype.aspx)

```C#
public enum TileTemplateType
{
    TileSquareImage = 0,
    TileSquareBlock = 1,
    TileSquareText01 = 2,
    TileSquareText02 = 3,
    TileSquareText03 = 4,
    TileSquareText04 = 5,
    TileSquarePeekImageAndText01 = 6,
    TileSquarePeekImageAndText02 = 7,
    TileSquarePeekImageAndText03 = 8,
    TileSquarePeekImageAndText04 = 9,
    TileWideImage = 10,
    TileWideImageCollection = 11,
    TileWideImageAndText01 = 12,
    TileWideImageAndText02 = 13,
    TileWideBlockAndText01 = 14,
    TileWideBlockAndText02 = 15,
    TileWidePeekImageCollection01 = 16,
    TileWidePeekImageCollection02 = 17,
    TileWidePeekImageCollection03 = 18,
    TileWidePeekImageCollection04 = 19,
    TileWidePeekImageCollection05 = 20,
    TileWidePeekImageCollection06 = 21,
    TileWidePeekImageAndText01 = 22,
    TileWidePeekImageAndText02 = 23,
    TileWidePeekImage01 = 24,
    TileWidePeekImage02 = 25,
    TileWidePeekImage03 = 26,
    TileWidePeekImage04 = 27,
    TileWidePeekImage05 = 28,
    TileWidePeekImage06 = 29,
    TileWideSmallImageAndText01 = 30,
    TileWideSmallImageAndText02 = 31,
    TileWideSmallImageAndText03 = 32,
    TileWideSmallImageAndText04 = 33,
    TileWideSmallImageAndText05 = 34,
    TileWideText01 = 35,
    TileWideText02 = 36,
    TileWideText03 = 37,
    TileWideText04 = 38,
    TileWideText05 = 39,
    TileWideText06 = 40,
    TileWideText07 = 41,
    TileWideText08 = 42,
    TileWideText09 = 43,
    TileWideText10 = 44,
    TileWideText11 = 45
}

public enum ToastTemplateType
{
    ToastImageAndText01,
    ToastImageAndText02,
    ToastImageAndText03,
    ToastImageAndText04,
    ToastText01,
    ToastText02,
    ToastText03,
    ToastText04
}

public enum PeriodicUpdateRecurrence
{
    HalfHour  = 0,
    Hour = 1,
    SixHours = 2,
    TwelveHours = 3,
    Daily =4
}
	
public enum SpeechResultStatus
{
    Command = 0, 
    Complete = 1,
    Dictation = 2,
    Hypothesis = 3
}
```	
### Stub/proxy classes

```C#
public class PushNotificationChannel
{
    public DateTime ExpirationTime { get; set; }
    public string Uri { get; set; }
}

public class SpeechArguments
{
    public SpeechResultStatus Status { get; set; }
    public string Text { get; set; }
}
	
```
	
### Core classes

```C#
public class Notifications
{
    public static void CreatePushNotificationChannelForApplication(Action<PushNotificationChannel, Exception> OnPushNotificationChannelCreated);
    public static void CreatePushNotificationChannelForSecondaryTile(string tileId, Action<PushNotificationChannel, Exception> OnPushNotificationChannelForSecondaryTileCreated);
    public static void RegisterForNotifications(Action<object> OnPushNotification, bool cancelDefaultBehavior);
}

public class RoamingSettings
{
    public static String RoamingFolder { get; set; }
    public static ulong RoamingStorageQuota { get; set; }
    public static event Action OnDataChanged;
    public static string[] AllContainerNames { get; }
    public static string[] AllKeys { get; }
    public static void ClearAllApplicationData(Action OnClearAppDataFinished);
    public static void SetValueForKey(string key, object value);
    public static void SetValueForKeyInContainer(string containerName, string key, object value);
    public static void DeleteContainer(string containerName);
    public static void DeleteValueForKey(string key);
    public static void DeleteValueForKeyInContainer(string containerName, string key);
    public static object GetValueForKey(string key);
    public static object GetValueForKey(string key, object defaultValue);
    public static object GetValueForKeyInContainer(string key);
    public static object GetValueForKeyInContainer(string containerName, string key);
    public static object GetValueForKeyInContainer(string containerName, string key, object defaultValue);
}

public class Speech
{
    public static void ListenForCommands(IEnumerable<string> commands, Action<SpeechArguments> OnSpeechResults);
    public static void ListenForDictation(Action<SpeechArguments> OnSpeechResults);
    public static void Stop();
}

public class TextUtils
{
    public static void WriteAllText(string fileName, string text);
    public static void WriteAllBytes(string fileName, byte[] bytes);
    public static string ReadAllText(string fileName);
    public static byte[] ReadAllBytes(string fileName);
}

public class Tiles
{
    public static void StartPeriodicUpdate(string url, PeriodicUpdateRecurrence periodicUpdateRecurrenceType);
    public static void StartPeriodicUpdate(string url, PeriodicUpdateRecurrence periodicUpdateRecurrenceType,
        DateTime startTime);
    public static void StopPeriodicUpdate();
    public static void enableNotificationQueue(bool enable);
    public static void UpdateTile(TileTemplateType tileTemplateType, string[] text);
    public static void UpdateTile(TileTemplateType tileTemplateType, string[] text, string[] images);
    public static void UpdateTile(string xml, DateTimeOffset? expirationTime);
    public static void UpdateTile(TileTemplateType tileTemplateType, string[] text, string[] images, DateTimeOffset? expirationTime);
    public static string GetTemplateContent(TileTemplateType tileTemplateType);
    public static void ClearTile();
    public static void CreateBadge(String value);
    public static void ClearBadge();
}
	
public class Toasts
{
    public static void ScheduleToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset deliveryTime)
    public static void ScheduleToast(ToastTemplateType toastTemplateType, string[] text, DateTimeOffset deliveryTime)
    public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image)
    public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset? expirationTime)
    public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset? expirationTime, Action<string> OnToastDismissed, Action OnToastActivated, Action<Exception> OnToastFailed)
}
	
```

### Samples
A sample is included in the [github repository](https://github.com/Microsoft/unityplugins) under *Samples/CoreTest* folder. Cortana samples are present under *Samples/Cortana*. A Windows Store exported project with the appropriate settings is present in the *Samples/CoreTest/out_win10* for Core and *Samples/Cortana/out_win10* folder for cortana.
