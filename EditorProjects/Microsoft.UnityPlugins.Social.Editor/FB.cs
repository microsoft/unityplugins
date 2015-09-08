using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{

    public delegate void FacebookDelegate(FBResult result);

    public delegate void InitDelegate();

    public delegate void HideUnityDelegate(bool hideUnity);

    public enum HttpMethod
    {
        GET,
        POST,
        DELETE
    }

    public class FBResult
    {
        public string Error { get; set; }
        public string Text { get; set; }
        public JsonObject Json { get; set; }
    }

    public class MissingPlatformException : Exception
    {
        public MissingPlatformException()
            : base("Platform components have not been set")
        {
        }
    }

    public static class FB
    {
        public static string UserId { get; private set; }
        public static string UserName { get; set; }
        public static bool IsLoggedIn { get { return false; } }
        public static string AppId { get; private set; }
        public static string AccessToken { get; private set; }
        public static DateTime Expires { get; private set; }

        // check whether facebook is initialized
        public static bool IsInitialized
        {
            get
            {
                return true;
            }
        }

        public static void Logout()
        {
        }

        public static void Login(string permissions, FacebookDelegate callback)
        {
        }


        public static void Init(
            InitDelegate onInitComplete,
            string appId,
            HideUnityDelegate onHideUnity, string redirectUrl = null)
        {
            onInitComplete();

            onHideUnity(false);
        }

        public static void ChangeRedirect(string redirectUrl)
        {
        }


        public static void API(
            string endpoint,
            HttpMethod method,
            FacebookDelegate callback)
        {
            if(callback != null)
                callback(new FBResult());
        }

        /// <summary>
        /// Show Request Dialog.
        /// to, title, data, filters, excludeIds and maxRecipients are not currently supported at this time.
        /// </summary>
        public static void AppRequest(
                string message,
                string[] to = null,
                string filters = "",
                string[] excludeIds = null,
                int? maxRecipients = null,
                string data = "",
                string title = "",
                FacebookDelegate callback = null
            )
        {
            if(callback != null)
            {
                callback(new FBResult());
            }
        }

        /// <summary>
        /// Show the Feed Dialog.
        /// mediaSource, actionName, actionLink, reference and properties are not currently supported at this time.
        /// </summary>
        public static void Feed(
            string toId = "",
            string link = "",
            string linkName = "",
            string linkCaption = "",
            string linkDescription = "",
            string picture = "",
            string mediaSource = "",
            string actionName = "",
            string actionLink = "",
            string reference = "",
            Dictionary<string, string[]> properties = null,
            FacebookDelegate callback = null)
        {
            if (callback != null)
            {
                callback(new FBResult());
            }
        }
    }
}

