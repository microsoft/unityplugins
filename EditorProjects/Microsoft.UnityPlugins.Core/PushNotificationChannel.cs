using System;

namespace Microsoft.UnityPlugins
{
    public class PushNotificationChannel
    {
        public DateTime ExpirationTime { get; set; }
        public string Uri { get; set; }
    }
}