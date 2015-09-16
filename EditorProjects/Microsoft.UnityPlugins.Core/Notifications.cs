using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            if(OnPushNotificationChannelCreated != null)
            {
                OnPushNotificationChannelCreated(new CallbackResponse<PushNotificationChannel> { Result = null, Status = CallbackStatus.Failure, Exception = new Exception("Windows Store APIs cannot be called in the editor") });
            }
        }

        /// <summary>
        /// Get Notification channel for secondary tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="OnPushNotificationChannelForSecondaryTileCreated"></param>
        public static void CreatePushNotificationChannelForSecondaryTile(string tileId, Action<CallbackResponse<PushNotificationChannel>> OnPushNotificationChannelForSecondaryTileCreated)
        {
            if (OnPushNotificationChannelForSecondaryTileCreated != null)
            {
                OnPushNotificationChannelForSecondaryTileCreated(new CallbackResponse<PushNotificationChannel> { Result = null, Status = CallbackStatus.Failure, Exception = new Exception("Windows Store APIs cannot be called in the editor") });
            }
        }

        /// <summary>
        /// Put an event handler for notifications.
        /// </summary>
        /// <param name="OnPushNotification"></param>
        /// <param name="cancelDefaultBehavior"></param>
        public static void RegisterForNotifications(Action<CallbackResponse<object>> OnPushNotification, bool cancelDefaultBehavior)
        {
            if(OnPushNotification != null)
            {
                OnPushNotification(new CallbackResponse<object> { Result = null, Status = CallbackStatus.Failure, Exception = new Exception("Windows Store APIs cannot be called in the editor") });
            }
        }
    }
}
