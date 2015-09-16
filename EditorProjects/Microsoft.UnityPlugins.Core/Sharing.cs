using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public enum SharingType
    {
        Text,
        Image,
        HTML
    }

    public class Sharing
    {
        // following data should be filled for all sharing types
        public static string Title;
        public static string Description;
        public static string Text;
        public static string Url;

        // For SharingType.Text, this MUST be filled
        public static string TextToShare;

        // For SharingType.Image, this MUST be filled
        // provide the image file path within the appx
        public static string ImageFilePath;

        // For SharingType HTML, this MUST be filled
        public static string HTMLContent;

        static Sharing()
        {

        }

        public static void ShowShareUI(SharingType sharingType, Action<CallbackResponse> OnShareCompleted)
        {
            OnShareCompleted(new CallbackResponse { Status = CallbackStatus.Failure, Exception = new Exception("Cannot call Windows Store APIs in Unity Editor")});
        }
    }
}
