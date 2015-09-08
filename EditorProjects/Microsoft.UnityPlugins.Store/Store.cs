using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    /// <summary>
    /// This library is here simply as a place holder to make the Unity editor happy. The main implementation is elsewhere
    /// </summary>
    public class Store
    {
        /// <summary>
        /// TODO: (sanjeevd) This function should be called from app.xaml.cs so it is always registered
        /// </summary>
        public static void RegisterForLicenseChangeEvents(Action OnLicenseChangedHandler)
        {
        }

        // If the app is in trial mode, returns  yes, else false
        public static bool IsInTrialMode()
        {
            return false;
        }

        public static void LoadListingInformation(Action<CallbackResponse<ListingInformation>> OnLoadListingFinished)
        {
            OnLoadListingFinished(new CallbackResponse<ListingInformation>());
        }

        public static void VerifyReceipt(string receipt, Action<CallbackResponse<ReceiptResponse>> OnReceiptVerified)
        {
            OnReceiptVerified(new CallbackResponse<ReceiptResponse>());
        }

        public static void VerifyReceipt(string receiptWebserviceUrl, string receipt, Action<CallbackResponse<ReceiptResponse>> OnReceiptVerified)
        {
            OnReceiptVerified(new CallbackResponse<ReceiptResponse>());
        }

        public static LicenseInformation GetLicenseInformation()
        {
            return new LicenseInformation();
        }

        public static ProductLicense GetProductLicense(string productId)
        {
            return new ProductLicense();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OnAppReceiptAcquired"></param>
        public static void GetAppReceipt(Action<CallbackResponse<string>> OnAppReceiptAcquired)
        {
            OnAppReceiptAcquired(new CallbackResponse<string>());
        }

        public static void GetProductReceipt(string productId, Action<CallbackResponse<string>> OnProductReceiptAcquired)
        {
            OnProductReceiptAcquired(new CallbackResponse<string>());
        }


        public static void LoadUnfulfilledConsumables(Action<CallbackResponse<List<UnfulfilledConsumable>>> OnLoadUnfulfilledConsumablesFinished)
        {
            OnLoadUnfulfilledConsumablesFinished(new CallbackResponse<List<UnfulfilledConsumable>>());
        }

        public static void ReportConsumableFulfillment(string productId, Guid transactionId,
            Action<CallbackResponse<FulfillmentResult>> OnReportConsumableFulfillmentFinished)
        {
            OnReportConsumableFulfillmentFinished(new CallbackResponse<FulfillmentResult>());
        }

        public static void RequestAppPurchase(bool requireReceipt, Action<CallbackResponse<string>> OnAppPurchaseFinished)
        {
            OnAppPurchaseFinished(new CallbackResponse<string>());
        }

        public static void RequestProductPurchase(string productId, Action<CallbackResponse<PurchaseResults>> OnProductPurchaseFinished)
        {
            OnProductPurchaseFinished(new CallbackResponse<PurchaseResults>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param> It is best not to set the callback parameter to null. The reason being  you don't know
        /// when the license loading is finished. Best set it properly to avoid tricky race conditions
        /// <param name="licenseFilePath"></param>
        /// 
        public static void LoadLicenseXMLFile(Action<CallbackResponse> callback, string licenseFilePath = null)
        {
            // invoke the callback for testing in editor
            callback(new CallbackResponse());
        }
    }
}
