using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{

    public class Toasts
    {
        public static void ScheduleToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset deliveryTime)
        {
        }

        public static void ScheduleToast(ToastTemplateType toastTemplateType, string[] text, DateTimeOffset deliveryTime)
        {
        }

        public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image)
        {
        }

        public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset? expirationTime)
        {
        }

        public static void ShowToast(ToastTemplateType toastTemplateType, string[] text, string image, DateTimeOffset? expirationTime, Action<string> OnToastDismissed, Action OnToastActivated, Action<Exception> OnToastFailed)
        {
        }

    }
}
