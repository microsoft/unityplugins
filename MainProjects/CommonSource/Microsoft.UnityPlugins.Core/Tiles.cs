using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Microsoft.UnityPlugins
{
    public class Tiles
    {
        public static void StartPeriodicUpdate(string url, PeriodicUpdateRecurrence periodicUpdateRecurrenceType)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().StartPeriodicUpdate(new Uri(url, UriKind.Absolute), (Windows.UI.Notifications.PeriodicUpdateRecurrence)periodicUpdateRecurrenceType);
        }

        public static void StartPeriodicUpdate(string url, PeriodicUpdateRecurrence periodicUpdateRecurrenceType,
            DateTime startTime)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().StartPeriodicUpdate(new Uri(url, UriKind.Absolute), startTime == null? DateTimeOffset.Now : new DateTimeOffset(startTime), (Windows.UI.Notifications.PeriodicUpdateRecurrence)periodicUpdateRecurrenceType);
        }

        public static void StopPeriodicUpdate()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().StopPeriodicUpdate();   
        }

        public static void enableNotificationQueue(bool enable)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
        }

        public static void UpdateTile(TileTemplateType tileTemplateType, string[] text)
        {
            UpdateTile(tileTemplateType, text, null, null);
        }

        public static void UpdateTile(TileTemplateType tileTemplateType, string[] text, string[] images)
        {
            UpdateTile(tileTemplateType, text, images, null);
        }

        public static void UpdateTile(string xml, DateTimeOffset? expirationTime)
        {
            var tileXml = new Windows.Data.Xml.Dom.XmlDocument();
            tileXml.LoadXml(xml);
            var notification = new TileNotification(tileXml);
            if (expirationTime != null)
            {
                if (expirationTime > DateTimeOffset.Now)
                {
                    notification.ExpirationTime = expirationTime;
                }
            }

            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        public static void UpdateTile(TileTemplateType tileTemplateType, string[] text, string[] images, DateTimeOffset? expirationTime)
        {
            // look for the templates on https://msdn.microsoft.com/en-us/library/windows/apps/Hh761491.aspx
            // all the nodes for supplying text are called "text". Conversely, everything with an image has a 
            // tag called "image". This simplistic function simply searches all the XML nodes with "text" and replaces 
            // them with the supplied text. Same for the images.
            var tileXml = TileUpdateManager.GetTemplateContent((Windows.UI.Notifications.TileTemplateType)tileTemplateType);

            if (text != null)
            {
                var textNodes = tileXml.GetElementsByTagName("text");
                if (textNodes.Length != text.Length)
                {
                    throw new ArgumentException("more text fields than supported by the template");
                }

                for (int i = 0; i < textNodes.Length; i++)
                {
                    textNodes[i].InnerText = text[i];
                }
            }

            // for a text only template, images will not be supplied.
            if (images != null)
            {
                var imageNodes = tileXml.GetElementsByTagName("image");
                if (images.Length != imageNodes.Length)
                {
                    throw new ArgumentException("more images supplied than images");
                }

                for (int j = 0; j < imageNodes.Length; j++)
                {
                    imageNodes[j].Attributes.GetNamedItem("src").NodeValue = images[j];
                }
            }

            var notification = new TileNotification(tileXml);
            if(expirationTime != null)
            {
                if(expirationTime > DateTimeOffset.Now)
                {
                    notification.ExpirationTime = expirationTime;
                }
            }

            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        public static string GetTemplateContent(TileTemplateType tileTemplateType)
        {
            var tileXml = TileUpdateManager.GetTemplateContent((Windows.UI.Notifications.TileTemplateType)tileTemplateType);
            return tileXml.GetXml();
        }

        public static void ClearTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

        public static void CreateBadge(String value)
        {
            var badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            var badgeAttributes = badgeXml.GetElementsByTagName("badge");
            badgeAttributes[0].Attributes.GetNamedItem("value").NodeValue = value;

            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(badgeXml));
        }

        public static void ClearBadge()
        {
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
        }
    }
}
