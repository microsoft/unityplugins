using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web;

namespace Microsoft.UnityPlugins
{
    public sealed partial class FBWebView : UserControl, IWebInterface
    {
        private NavigationEventCallback _onFinished;
        private NavigationErrorCallback _onError;
        private NavigationEventCallback _onStart;
        private object _state;

        public FBWebView()
        {
            this.InitializeComponent();

            progressBar.Visibility = Visibility.Collapsed;
            progressBar.IsIndeterminate = false;

            web.NavigationStarting += HandleNavigationStarting;
            web.NavigationCompleted += HandleNavigationCompleted;
        }

        private void HandleNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            progressBar.Visibility = Visibility.Collapsed;
            progressBar.IsIndeterminate = false;

            var uri = args.Uri;
            var error = args.WebErrorStatus;
            if (args.IsSuccess)
            {
                Utils.RunOnUnityAppThread(() =>
                {
                    if (_onFinished != null)
                        _onFinished(uri, _state);
                });
            }
            else
            {
                Utils.RunOnUnityAppThread(() =>
                {
                    if (_onError != null)
                        _onError(uri, (int)error, _state);
                });
            }
        }

        private void HandleNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            progressBar.Visibility = Visibility.Visible;
            progressBar.IsIndeterminate = true;

            var uri = args.Uri;
            Utils.RunOnUnityAppThread(() =>
            {
                if (_onStart != null)
                    _onStart(uri, _state);
            });
        }

        public void Cancel()
        {
            if (IsActive)
            {
                Utils.RunOnUnityAppThread(() =>
                {
                    if (_onError != null)
                        _onError(null, -1, _state);
                    Finish();
                });

            }
        }

        public void Finish()
        {
            Utils.RunOnWindowsUIThread(() =>
            {
                _onError = null;
                _onStart = null;
                _onFinished = null;
                web.Stop();
                web.NavigateToString("");
                this.Visibility = Visibility.Collapsed;
                // TODO: (Fix this line..what does it really do?)
                //UnityPlayer.AppCallbacks.Instance.UnitySetInput(true);
                IsActive = false;
            });
        }

        public bool IsActive
        {
            get;
            private set;
        }

        public void Navigate(
            Uri uri,
            bool showUi,
            NavigationEventCallback finishedCallback,
            NavigationErrorCallback onError,
            object state = null,
            NavigationEventCallback startedCallback = null)
        {
            _onFinished = finishedCallback;
            _onStart = startedCallback;
            _onError = onError;

            _state = state;

            Utils.RunOnWindowsUIThread(() =>
            {
                this.Visibility = showUi ? Visibility.Visible : Visibility.Collapsed;
                // TODO: (Fix this)
                //if (showUi)
                //    UnityPlayer.AppCallbacks.Instance.UnitySetInput(false);
                IsActive = true;
                web.Navigate(uri);
            });
        }

        void backButton_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void ClearCookies()
        {
            var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookies = filter.CookieManager;
            var jar = cookies.GetCookies(new Uri("https://www.facebook.com"));
            foreach (var c in jar)
                cookies.DeleteCookie(c);
        }
    }
}
