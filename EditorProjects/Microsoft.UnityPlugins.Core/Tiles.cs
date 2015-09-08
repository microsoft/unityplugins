using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class Tiles
    {
        public static void StartPeriodicUpdate(string url, PeriodicUpdateRecurrence periodicUpdateRecurrenceType)
        {
        }

        public static void StartPeriodicUpdate(string url, PeriodicUpdateRecurrence periodicUpdateRecurrenceType,
            DateTime startTime)
        {
        }

        public static void StopPeriodicUpdate()
        {
        }

        public static void enableNotificationQueue(bool enable)
        {
        }

        public static void UpdateTile(TileTemplateType tileTemplateType, string[] text)
        {
        }

        public static void UpdateTile(TileTemplateType tileTemplateType, string[] text, string[] images)
        {
        }

        public static void UpdateTile(string xml, DateTimeOffset? expirationTime)
        {
        }

        public static void UpdateTile(TileTemplateType tileTemplateType, string[] text, string[] images, DateTimeOffset? expirationTime)
        {
        }

        public static string GetTemplateContent(TileTemplateType tileTemplateType)
        {
            return String.Empty;
        }

        public static void ClearTile()
        {
        }

        public static void CreateBadge(String value)
        {
        }

        public static void ClearBadge()
        {
        }
    }
}
