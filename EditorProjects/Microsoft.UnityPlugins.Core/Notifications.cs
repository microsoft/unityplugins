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
        public static void CreatePushNotificationChannelForApplication(Action<PushNotificationChannel, Exception> OnPushNotificationChannelCreated)
        {
            if(OnPushNotificationChannelCreated != null)
            {
                OnPushNotificationChannelCreated(null, null);
            }
        }

        /// <summary>
        /// Get Notification channel for secondary tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="OnPushNotificationChannelForSecondaryTileCreated"></param>
        public static void CreatePushNotificationChannelForSecondaryTile(string tileId, Action<PushNotificationChannel, Exception> OnPushNotificationChannelForSecondaryTileCreated)
        {
            if (OnPushNotificationChannelForSecondaryTileCreated != null)
            {
                OnPushNotificationChannelForSecondaryTileCreated(null, null);
            }
        }

        /// <summary>
        /// Put an event handler for notifications.
        /// </summary>
        /// <param name="OnPushNotification"></param>
        /// <param name="cancelDefaultBehavior"></param>
        public static void RegisterForNotifications(Action<object> OnPushNotification, bool cancelDefaultBehavior)
        {
            if(OnPushNotification != null)
            {
                OnPushNotification(null);
            }
        }
    }
}
