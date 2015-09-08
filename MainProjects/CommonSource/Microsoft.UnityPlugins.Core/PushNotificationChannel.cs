using System;

namespace Microsoft.UnityPlugins
{
    public class PushNotificationChannel
    {
        public DateTime ExpirationTime { get; set; }
        public string Uri { get; set; }

        public PushNotificationChannel(Windows.Networking.PushNotifications.PushNotificationChannel channel)
        {
            // Convert DateTimeOffset to DateTime to ensure things work with Unity
            ExpirationTime = channel.ExpirationTime.DateTime;
            Uri = channel.Uri;
        }
    }
}