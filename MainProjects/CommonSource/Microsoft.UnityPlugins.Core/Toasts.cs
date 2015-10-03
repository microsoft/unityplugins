using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Microsoft.UnityPlugins
{

    public class Toasts
    {
        public static void ScheduleToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset deliveryTime)
        {
            var toastXml = CreateToastNotificationXml(toastTemplateType, text, image);

            var notification = new ScheduledToastNotification(toastXml, deliveryTime);

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(notification);
        }

        public static void ScheduleToast(ToastTemplateType toastTemplateType, string[] text, DateTimeOffset deliveryTime)
        {
            ScheduleToast(toastTemplateType, text, null, deliveryTime);
        }

        public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image)
        {
            ShowToast(toastTemplateType, text, image, null, null, null, null);
        }

        public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset? expirationTime)
        {
            ShowToast(toastTemplateType, text, image, expirationTime, null, null, null);
        }

        public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset? expirationTime, Action<string> OnToastDismissed, Action OnToastActivated, Action<Exception> OnToastFailed)
        {
            Windows.Data.Xml.Dom.XmlDocument toastXml = CreateToastNotificationXml(toastTemplateType, text, image);

            var notification = new ToastNotification(toastXml);
            if (OnToastDismissed != null)
            {
                notification.Dismissed += (sender, args) =>
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        OnToastDismissed(args.Reason.ToString());
                    });
                };
            }

            if (OnToastActivated != null)
            {
                notification.Activated += (sender, args) =>
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        OnToastActivated();
                    });
                };
            }


            if (OnToastFailed != null)
            {
                notification.Failed += (sender, args) =>
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        OnToastFailed(args.ErrorCode);
                    });
                };
            }


            if (expirationTime != null)
            {
                if (expirationTime > DateTimeOffset.Now)
                {
                    notification.ExpirationTime = expirationTime;
                }
            }

            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        private static Windows.Data.Xml.Dom.XmlDocument CreateToastNotificationXml(ToastTemplateType toastTemplateType, string[] text, string image)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent((Windows.UI.Notifications.ToastTemplateType)toastTemplateType);
            var textNodes = toastXml.GetElementsByTagName("text");
            if (textNodes.Length != text.Length)
            {
                throw new ArgumentException("more text fields than supported by the template");
            }

            for (int i = 0; i < textNodes.Length; i++)
            {
                textNodes[i].InnerText = text[i];
            }

            // for a text only template, images will not be supplied.
            if ( !string.IsNullOrEmpty(image))
            {
                var imageNodes = toastXml.GetElementsByTagName("image");
                if (imageNodes.Length != 1)
                {
                    throw new ArgumentException("invalid number of images");
                }

                imageNodes[0].Attributes.GetNamedItem("src").NodeValue = image;
            }

            return toastXml;
        }
    }
}
