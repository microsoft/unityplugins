using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{
    public class Notifications
    {
        /// <summary>
        /// Get push notification channel for the application
        /// </summary>
        /// <param name="OnPushNotificationChannelCreated"></param>
        public static void CreatePushNotificationChannelForApplication(Action<CallbackResponse<PushNotificationChannel>> OnPushNotificationChannelCreated)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    if (OnPushNotificationChannelCreated != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                        {
                            OnPushNotificationChannelCreated(new CallbackResponse<PushNotificationChannel> { Result = new PushNotificationChannel(channel), Status = CallbackStatus.Success, Exception = null});
                        });
                    }
                }
                catch (Exception ex)
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        // If we caught an exception, send it back to the caller
                        OnPushNotificationChannelCreated(new CallbackResponse<PushNotificationChannel> { Result = null, Status = CallbackStatus.Failure, Exception = ex});
                    });

                    return;
                }
            });
        }

        /// <summary>
        /// Get Notification channel for secondary tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="OnPushNotificationChannelForSecondaryTileCreated"></param>
        public static void CreatePushNotificationChannelForSecondaryTile(string tileId, Action<CallbackResponse<PushNotificationChannel>> OnPushNotificationChannelForSecondaryTileCreated)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForSecondaryTileAsync(tileId);
                    if (OnPushNotificationChannelForSecondaryTileCreated != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                        {
                            OnPushNotificationChannelForSecondaryTileCreated(new CallbackResponse<PushNotificationChannel> { Result = new PushNotificationChannel(channel), Status = CallbackStatus.Success, Exception = null});
                        });
                    }
                }
                catch (Exception ex)
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        // If we caught an exception, send it back to the caller
                        OnPushNotificationChannelForSecondaryTileCreated(new CallbackResponse<PushNotificationChannel> { Result = null, Status = CallbackStatus.Failure, Exception = ex });
                    });

                    return;
                }
            });
        }

        /// <summary>
        /// Put an event handler for notifications.
        /// </summary>
        /// <param name="OnPushNotification"></param>
        /// <param name="cancelDefaultBehavior"></param>
        public static void RegisterForNotifications(Action<CallbackResponse<object>> OnPushNotification, bool cancelDefaultBehavior)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    channel.PushNotificationReceived += (s, e) =>
                    {
                        String notificationContent = String.Empty;

                        switch (e.NotificationType)
                        {
                            case Windows.Networking.PushNotifications.PushNotificationType.Badge:
                                notificationContent = e.BadgeNotification.Content.GetXml();
                                break;

                            case Windows.Networking.PushNotifications.PushNotificationType.Tile:
                                notificationContent = e.TileNotification.Content.GetXml();
                                break;

                            case Windows.Networking.PushNotifications.PushNotificationType.Toast:
                                notificationContent = e.ToastNotification.Content.GetXml();
                                break;

                            case Windows.Networking.PushNotifications.PushNotificationType.Raw:
                                notificationContent = e.RawNotification.Content;
                                break;
                        }

                        if (cancelDefaultBehavior)
                        {
                            e.Cancel = true;
                        }

                        Utils.RunOnUnityAppThread(() =>
                        {
                            if(OnPushNotification != null)
                                OnPushNotification(new CallbackResponse<object> { Result = notificationContent, Status = CallbackStatus.Success, Exception = null});
                        });
                    };
                }
                catch (Exception ex)
                {
                    DebugLog.Log(LogLevel.Error, "Error registering for notifications");
                    if (OnPushNotification != null)
                        OnPushNotification(new CallbackResponse<object> { Result = null, Status = CallbackStatus.Failure, Exception = ex});

                    return;
                }
            });
        }
    }
}
