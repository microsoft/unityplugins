using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

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
        public static string ImageFilePath;

        // For SharingType HTML, this MUST be filled
        public static string HTMLContent;

        private static Action<CallbackResponse> _onShareCompleted;
        private static SharingType _sharingType;

        static Sharing()
        {
            // Setup the event handler for data request
            DataTransferManager.GetForCurrentView().DataRequested += 
                new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(Sharing.OnDataRequested); ;
        }

        async private static void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            bool didSucceedOperation = false;
            var request = args.Request;

            switch(_sharingType)
            {
                case SharingType.Text:
                    {
                        if (String.IsNullOrEmpty(TextToShare))
                        {
                            request.FailWithDisplayText("Enter the text you would like to share and try again.");
                            break;
                        }

                        DataPackage requestData = request.Data;
                        requestData.Properties.Title = String.IsNullOrEmpty(Title)? String.Empty: Title;
                        requestData.Properties.Description = String.IsNullOrEmpty(Description) ? String.Empty : Description; 
                        requestData.Properties.ContentSourceApplicationLink = String.IsNullOrEmpty(Url) ? new Uri(String.Empty) : new Uri(Url, UriKind.RelativeOrAbsolute);
                        requestData.SetText(String.IsNullOrEmpty(TextToShare) ? String.Empty : TextToShare);

                        didSucceedOperation = true;
                        break;
                    }
                case SharingType.Image:
                    {
                        StorageFile imageFile = await Package.Current.InstalledLocation.GetFileAsync(ImageFilePath);

                        if (imageFile == null)
                        {
                            didSucceedOperation = false;
                            return;
                        }

                        DataPackage requestData = request.Data;

                        List<IStorageItem> imageItems = new List<IStorageItem>();
                        imageItems.Add(imageFile);
                        requestData.SetStorageItems(imageItems);

                        RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(imageFile);
                        requestData.Properties.Thumbnail = imageStreamRef;
                        requestData.SetBitmap(imageStreamRef);

                        didSucceedOperation = true;
                        break;
                    }
                case SharingType.HTML:
                    {
                        // Get the user's selection from the WebView. Since this is an asynchronous operation we need to acquire the deferral first.
                        DataRequestDeferral deferral = request.GetDeferral();

                        // Make sure to always call Complete when done with the deferral.
                        try
                        {

                            DataPackage requestData = request.Data;
                                requestData.Properties.Title = "A web snippet for you";
                                requestData.Properties.Description = "HTML selection from a WebView control"; // The description is optional.
                                requestData.Properties.ContentSourceApplicationLink = String.IsNullOrEmpty(Url) ? new Uri(String.Empty) : new Uri(Url, UriKind.RelativeOrAbsolute);
                                string data = Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.CreateHtmlFormat(HTMLContent);
                                request.Data.SetHtmlFormat(data);
                                deferral.Complete();

                            didSucceedOperation = true;
                        }
                        catch (Exception)
                        {
                            deferral.Complete();
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (!didSucceedOperation)
            {
                if (_onShareCompleted != null)
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        _onShareCompleted(new CallbackResponse { Exception = new Exception("Sharing failed because parameters are not set correctly"), Status = CallbackStatus.Failure });
                        return;
                    });
                }
            }
        }

        public static void ShowShareUI(SharingType sharingType, Action<CallbackResponse> OnShareCompleted)
        {
            _sharingType = sharingType;
            _onShareCompleted = OnShareCompleted;
            Utils.RunOnWindowsUIThread(() =>
            {
                try
                {
                    DataTransferManager.ShowShareUI();
                }
                catch(Exception ex)
                {
                    if(OnShareCompleted != null)
                    {
                        OnShareCompleted(new CallbackResponse { Exception = ex, Status = CallbackStatus.Failure});
                        return;
                    }
                }
            });
        }
    }
}
