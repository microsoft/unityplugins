using System.Threading.Tasks;
using System.Globalization;
using Facebook;
using System;
using System.Collections.Generic;

using Facebook.Client;
using Windows.Storage;
using System.Linq;
namespace Microsoft.UnityPlugins
{

    /// <summary>
    /// Unity Facebook implementation
    /// </summary>
    public static class FB
    {

#if NETFX_CORE
        private static Session _fbSessionClient;
        private static HideUnityDelegate _onHideUnity;
#endif

        /// <summary>
        /// FB.Init as per Unity SDK
        /// </summary>
        /// <remarks>
        /// https://developers.facebook.com/docs/unity/reference/current/FB.Init
        /// </remarks>
        public static void Init(InitDelegate onInitComplete, string appId, HideUnityDelegate onHideUnity)
        {
            Utils.RunOnWindowsUIThread(() =>
            {
                _onHideUnity = onHideUnity;
                _fbSessionClient = Session.ActiveSession;
                Session.AppId = appId;
                Task.Run(() =>
                {
                    // TODO: Make check and extend token available to Windows
                    // await Session.CheckAndExtendTokenIfNeeded();
                    if (onInitComplete != null)
                        Utils.RunOnUnityAppThread(() => { onInitComplete(); });
                });

                if (onHideUnity != null)
                    throw new NotSupportedException("onHideUnity is not currently supported at this time.");
            });

        }

        public static void Logout()
        {
            _fbSessionClient.Logout();
        }

        public static void Login(string permissions, FacebookDelegate callback)
        {
            Session.OnFacebookAuthenticationFinished = (AccessTokenData data) =>
            {
                if (callback != null)
                    Utils.RunOnUnityAppThread(() => { callback(new FBResult() { Text = (data == null || String.IsNullOrEmpty(data.AccessToken)) ? "Fail" : "Success", Error = (data == null || String.IsNullOrEmpty(data.AccessToken)) ? "Error" : null }); });
            };

            Utils.RunOnWindowsUIThread(() =>
            {
                _fbSessionClient.LoginWithBehavior(permissions, FacebookLoginBehavior.LoginBehaviorMobileInternetExplorerOnly);
            });
        }

        public static void API(
            string endpoint,
            HttpMethod method,
            FacebookDelegate callback)
        {

            if (method != HttpMethod.GET) throw new NotImplementedException();
            Task.Run(async () =>
            {
                FacebookClient fb = new FacebookClient(_fbSessionClient.CurrentAccessTokenData.AccessToken);
                FBResult fbResult = null;
                try
                {
                    var apiCall = await fb.GetTaskAsync(endpoint, null);
                    if (apiCall != null)
                    {
                        fbResult = new FBResult();
                        fbResult.Text = apiCall.ToString();
                        fbResult.Json = apiCall as JsonObject;
                    }
                }
                catch (Exception ex)
                {
                    fbResult = new FBResult();
                    fbResult.Error = ex.Message;
                }
                if (callback != null)
                {
                    Utils.RunOnUnityAppThread(() => { callback(fbResult); });
                }
            });
        }

        /// <summary>
        /// Get current logged in user info
        /// </summary>
        public static void GetCurrentUser(Action<FBUser> callback)
        {
            API("me", HttpMethod.GET, (result) =>
            {
                var data = (IDictionary<string, object>)result.Json;
                var me = new GraphUser(data);

                if (callback != null)
                    callback(new FBUser(me));
            });
        }

        /// <summary>
        /// Show Request Dialog.
        /// filters, excludeIds and maxRecipients are not currently supported at this time.
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
            if (!IsLoggedIn)
            {
                // not logged in
                if (callback != null)
                    Utils.RunOnUnityAppThread(() => { callback(new FBResult() { Error = "Not Logged In" }); });
                return;
            }

            Session.OnFacebookAppRequestFinished = (result) =>
            {
                if (callback != null)
                    Utils.RunOnUnityAppThread(() => { callback(new FBResult() { Text = result.Text, Error = result.Error, Json = result.Json }); });
            };

            // pass in params to facebook client's app request
            Utils.RunOnWindowsUIThread(() =>
            {
                //Session.ShowAppRequestsDialog(message, title, to != null ? to.ToList<string>() : null, data);
                // TODO: (fix app requests to take in various other parameters)
                Session.ShowAppRequestsDialog((arg) => { }, message, title, to.AsEnumerable<string>().ToList<string>());
            });

            // throw not supported exception when user passed in parameters not supported currently
            if (!string.IsNullOrWhiteSpace(filters) || excludeIds != null || maxRecipients != null)
                throw new NotSupportedException("filters, excludeIds and maxRecipients are not currently supported at this time.");
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
            if (!IsLoggedIn)
            {
                // not logged in
                if (callback != null)
                    Utils.RunOnUnityAppThread(() => { callback(new FBResult() { Error = "Not Logged In" }); });
                return;
            }

            Session.OnFacebookFeedFinished = (result) =>
            {
                if (callback != null)
                    Utils.RunOnUnityAppThread(() => { callback(new FBResult() { Text = result.Text, Error = result.Error, Json = result.Json }); });
            };

            // pass in params to facebook client's app request
            Utils.RunOnWindowsUIThread(() =>
            {
                //Session.ShowFeedDialogViaBrowser(toId, link, linkName, linkCaption, linkDescription, picture);
                Session.ShowFeedDialog(toId, link, linkName, linkCaption, linkDescription, picture);
            });

            // throw not supported exception when user passed in parameters not supported currently
            if (!string.IsNullOrWhiteSpace(mediaSource) || !string.IsNullOrWhiteSpace(actionName) || !string.IsNullOrWhiteSpace(actionLink) ||
                !string.IsNullOrWhiteSpace(reference) || properties != null)
                throw new NotSupportedException("mediaSource, actionName, actionLink, reference and properties are not currently supported at this time.");
        }

        // additional methods added for convenience

        public static bool IsLoggedIn
        {
            get
            {
                return _fbSessionClient != null && !String.IsNullOrEmpty(_fbSessionClient.CurrentAccessTokenData.AccessToken);
            }
        }

        public static bool BackButtonPressed()
        {
            if (_fbSessionClient != null && Session.IsDialogOpen)
            {
                Session.CloseWebDialog();
                return true;
            }
            else
                return false;
        }

        // check whether facebook is initialized
        public static bool IsInitialized
        {
            get
            {
                return _fbSessionClient != null;
            }
        }
    }
}