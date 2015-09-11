//
//  Author: (Sanjeev Dwivedi), 
//  Revision 1.0 7/7/2015
//  Unity Plugin for store related functionality.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace Microsoft.UnityPlugins
{
    public class Store
    {
        private static bool _isLicenseSimulationOn = false;

        /// <summary>
        /// This event will be invoked when a license changes. Really, this should be invoked only when a trial license expires.
        /// </summary>

        private static Action _onLicenseChanged;

        /// <summary>
        /// </summary>
        public static void RegisterForLicenseChangeEvents(Action OnLicenseChangedHandler)
        {
            if (OnLicenseChangedHandler != null)
            {
                _onLicenseChanged += OnLicenseChangedHandler;
            }

            Utils.RunOnWindowsUIThread(() =>
            {
                if (_isLicenseSimulationOn)
                {
                    CurrentAppSimulator.LicenseInformation.LicenseChanged += LicenseInformation_LicenseChanged;
                }
                else
                {
                    CurrentApp.LicenseInformation.LicenseChanged += LicenseInformation_LicenseChanged;
                }
            });
        }

        private static void LicenseInformation_LicenseChanged()
        {
            Utils.RunOnUnityAppThread(() =>
            {
                if (_onLicenseChanged != null)
                {
                    _onLicenseChanged();
                }
            });
        }

        // If the app is in trial mode, returns  yes, else false
        public static bool IsInTrialMode()
        {
            if (_isLicenseSimulationOn)
            {
                return CurrentAppSimulator.LicenseInformation.IsTrial;
            }
            else
            {
                return CurrentApp.LicenseInformation.IsTrial;
            }
        }

        private static ListingInformation _listingInformation = null;
        public static void LoadListingInformation(Action<CallbackResponse<ListingInformation>> OnLoadListingFinished)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    if (_isLicenseSimulationOn)
                    {
                        _listingInformation = new ListingInformation(await CurrentAppSimulator.LoadListingInformationAsync());
                    }
                    else
                    {
                        _listingInformation = new ListingInformation(await CurrentApp.LoadListingInformationAsync());
                    }
                }
                catch(Exception ex)
                {
                    if (OnLoadListingFinished != null)
                    {
                        Utils.RunOnUnityAppThread(() => { OnLoadListingFinished(new CallbackResponse<ListingInformation> { Result = null, Status = CallbackStatus.Failure, Exception = ex }); });
                        return;
                    }
                }

                // This must get invoked on the Unity thread.
                // On successful completion, invoke the OnLoadListingFinished event handler
                if (OnLoadListingFinished != null && _listingInformation != null)
                {
                    Utils.RunOnUnityAppThread(() => { OnLoadListingFinished(new CallbackResponse<ListingInformation> { Result = _listingInformation, Status = CallbackStatus.Success, Exception = null }); });
                }
            });
        }

        public static LicenseInformation GetLicenseInformation()
        {
            if (_isLicenseSimulationOn)
            {
                return new LicenseInformation(CurrentAppSimulator.LicenseInformation);
            }
            else
            {
                return new LicenseInformation(CurrentApp.LicenseInformation);
            }
        }

        public static ProductLicense GetProductLicense(string productId)
        {
            if (CurrentAppSimulator.LicenseInformation.ProductLicenses.ContainsKey(productId))
            {
                return new ProductLicense(CurrentAppSimulator.LicenseInformation.ProductLicenses[productId]);
            }

            return  new ProductLicense(CurrentApp.LicenseInformation.ProductLicenses[productId]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OnAppReceiptAcquired"></param>
        public static void GetAppReceipt(Action<CallbackResponse<string>> OnAppReceiptAcquired)
        {
            string receipt = String.Empty;
            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    if (_isLicenseSimulationOn)
                    {
                        receipt = await CurrentAppSimulator.GetAppReceiptAsync();
                    }
                    else
                    {
                        receipt = await CurrentApp.GetAppReceiptAsync();
                    }
                }
                catch(Exception ex)
                {
                    Utils.RunOnUnityAppThread(() => { if (OnAppReceiptAcquired != null) OnAppReceiptAcquired(new CallbackResponse<string> { Result = null, Exception = ex, Status = CallbackStatus.Failure }); });
                    return;
                }

                Utils.RunOnUnityAppThread(() => { if(OnAppReceiptAcquired != null) OnAppReceiptAcquired(new CallbackResponse<string> { Result = receipt, Exception = null, Status = CallbackStatus.Success }); });
            });
        }

        public static void VerifyReceipt(string receiptWebserviceUrl, string receipt, Action<CallbackResponse<ReceiptResponse>> OnReceiptVerified)
        {
            Task.Run(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var content = new StringContent(receipt, Encoding.UTF8, "application/xml");

                    DebugLog.Log(LogLevel.Info, "Sending out receipt for verification");

                    
                    var response = await httpClient.PostAsync(new Uri(receiptWebserviceUrl, UriKind.RelativeOrAbsolute), content);

                    var responseString = await response.Content.ReadAsStringAsync();
                    DebugLog.Log(LogLevel.Info, "received response: " + responseString);
                    var responseObject = JsonConvert.DeserializeObject<ReceiptResponse>(responseString);

                    // TODO: Is this right? We created a new thread to accommodate the async/await.. Then we marshal the callback back to the app thread
                    if (OnReceiptVerified != null)
                    {
                        Utils.RunOnUnityAppThread(() => OnReceiptVerified(new CallbackResponse<ReceiptResponse> { Result = responseObject, Exception = null, Status = CallbackStatus.Success }));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    if (OnReceiptVerified != null)
                    {
                        // return the results back on the Unity app thread
                        Utils.RunOnUnityAppThread(() => OnReceiptVerified(new CallbackResponse<ReceiptResponse> { Result = null, Exception = ex, Status = CallbackStatus.Failure }));
                        return;
                    }
                }
            });
        }

        public static void VerifyReceipt(string receipt, Action<CallbackResponse<ReceiptResponse>> OnReceiptVerified)
        {
            // TODO: switch the final receipt verification webservice here
            VerifyReceipt("http://sanjeevdsignatureverification.azurewebsites.net/api/verify", receipt, OnReceiptVerified);
        }

        public static void GetProductReceipt(string productId, Action<CallbackResponse<string>> OnProductReceiptAcquired)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                String receipt = String.Empty;

                // exceptions raised on the UI thread will be delivered on the UI thread.. so catch exception that is happening on
                // the UI thread, and marshal it back to the Unity app thread.
                try
                {
                    if (_isLicenseSimulationOn)
                    {
                        receipt = await CurrentAppSimulator.GetProductReceiptAsync(productId);
                    }
                    else
                    {
                        receipt = await CurrentApp.GetProductReceiptAsync(productId);
                    }
                }
                catch(Exception ex)
                {
                    Utils.RunOnUnityAppThread(() => { if (OnProductReceiptAcquired != null) OnProductReceiptAcquired(new CallbackResponse<string> { Result = null, Exception = ex, Status = CallbackStatus.Failure}); });
                    return;
                }

                Utils.RunOnUnityAppThread(() => { if (OnProductReceiptAcquired != null) OnProductReceiptAcquired(new CallbackResponse<string> { Result = receipt, Exception = null, Status = CallbackStatus.Success }); });
            });
        }


        public static void LoadUnfulfilledConsumables(Action<CallbackResponse<List<UnfulfilledConsumable>>> OnLoadUnfulfilledConsumablesFinished)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                IReadOnlyList<Windows.ApplicationModel.Store.UnfulfilledConsumable> unfulfilledConsumables = null;
                List<UnfulfilledConsumable> deliveryFormatUnfulfilledConsumables = new List<UnfulfilledConsumable>();
                try
                {
                    if (_isLicenseSimulationOn)
                    {
                        unfulfilledConsumables = await CurrentAppSimulator.GetUnfulfilledConsumablesAsync();
                    }
                    else
                    {
                        unfulfilledConsumables = await CurrentApp.GetUnfulfilledConsumablesAsync();
                    }

                    
                    foreach (var oneUnfConsumable in unfulfilledConsumables)
                    {
                        deliveryFormatUnfulfilledConsumables.Add(new UnfulfilledConsumable(oneUnfConsumable));
                    }

                }
                catch (Exception ex)
                {
                    DebugLog.Log(LogLevel.Error, "Error while reporting consumable fulfillment " + ex.ToString());
                    Utils.RunOnUnityAppThread(() => { OnLoadUnfulfilledConsumablesFinished(new CallbackResponse<List<UnfulfilledConsumable>> { Status = CallbackStatus.Failure, Exception = ex, Result = null }); });
                    return;
                }
                
                // deliver all the unfulfilled consumables at once
                Utils.RunOnUnityAppThread(() => { if (OnLoadUnfulfilledConsumablesFinished != null) OnLoadUnfulfilledConsumablesFinished(new CallbackResponse<List<UnfulfilledConsumable>> { Status = CallbackStatus.Success, Result = deliveryFormatUnfulfilledConsumables, Exception = null }); });
            });
        }

        public static void ReportConsumableFulfillment(string productId, Guid transactionId,
            Action<CallbackResponse<FulfillmentResult>> OnReportConsumableFulfillmentFinished)
        {

            Utils.RunOnWindowsUIThread(async () =>
            {
                Windows.ApplicationModel.Store.FulfillmentResult result = Windows.ApplicationModel.Store.FulfillmentResult.ServerError;
                try
                {

                    if (_isLicenseSimulationOn)
                    {
                        result = await CurrentAppSimulator.ReportConsumableFulfillmentAsync(productId, transactionId);
                    }
                    else
                    {
                        result = await CurrentApp.ReportConsumableFulfillmentAsync(productId, transactionId);
                    }

                }
                catch (Exception ex)
                {
                    DebugLog.Log(LogLevel.Error, "Error while reporting consumable fulfillment " + ex.ToString());
                    Utils.RunOnUnityAppThread(() => { if (OnReportConsumableFulfillmentFinished != null) OnReportConsumableFulfillmentFinished(new CallbackResponse<FulfillmentResult> { Status = CallbackStatus.Failure, Exception = ex, Result = FulfillmentResult.ServerError }); });
                    return;
                }

                // This should not really be throwing exceptions.. If it does, they will be raised on the Unity thread anyways, so game should handle it
                Utils.RunOnUnityAppThread(() =>
                {
                    if (OnReportConsumableFulfillmentFinished != null)
                        OnReportConsumableFulfillmentFinished(
                        new CallbackResponse<FulfillmentResult>
                        {
                            Result = (Microsoft.UnityPlugins.FulfillmentResult)result,
                            Exception = null,
                            Status = CallbackStatus.Success
                        });
                });
            });

        }

        public static void RequestAppPurchase(bool requireReceipt, Action<CallbackResponse<string>> OnAppPurchaseFinished)
        {

            string result = String.Empty;
            bool didPurchaseSucceed = false;
            Utils.RunOnWindowsUIThread(async () =>
                {
                    try
                    {

                        if (_isLicenseSimulationOn)
                        {
                            result = await CurrentAppSimulator.RequestAppPurchaseAsync(requireReceipt);
                            if (CurrentAppSimulator.LicenseInformation.IsActive && !CurrentAppSimulator.LicenseInformation.IsTrial)
                            {
                                didPurchaseSucceed = true;
                            }
                        }
                        else
                        {
                            result = await CurrentApp.RequestAppPurchaseAsync(requireReceipt);
                            if (CurrentApp.LicenseInformation.IsActive && !CurrentApp.LicenseInformation.IsTrial)
                            {
                                didPurchaseSucceed = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugLog.Log(LogLevel.Error, "Error purchasing the app " + ex.ToString());
                        Utils.RunOnUnityAppThread(() =>
                        {
                            if (OnAppPurchaseFinished != null)
                                OnAppPurchaseFinished(
                                new CallbackResponse<string>
                                {
                                    Status = CallbackStatus.Failure,
                                    Result = null,
                                    Exception = ex
                                });
                         });

                        return;
                    }

                    Utils.RunOnUnityAppThread(() =>
                    {
                        if (OnAppPurchaseFinished != null)
                            OnAppPurchaseFinished(new CallbackResponse<string>
                            {
                                Status = didPurchaseSucceed ? CallbackStatus.Success : CallbackStatus.Failure,
                                Result = result,
                                Exception = null
                            });
                    });
                });
        }

        public static void RequestProductPurchase(string productId, Action<CallbackResponse<PurchaseResults>> OnProductPurchaseFinished)
        {
            PurchaseResults result = null;
            Utils.RunOnWindowsUIThread(async () =>
                {
                    try
                    {

                        if (_isLicenseSimulationOn)
                        {
                            result = new PurchaseResults(await CurrentAppSimulator.RequestProductPurchaseAsync(productId));
                        }
                        else
                        {
                            result = new PurchaseResults(await CurrentApp.RequestProductPurchaseAsync(productId));
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugLog.Log(LogLevel.Error, "Error purchasing the product " + ex.ToString());
                        Utils.RunOnUnityAppThread(() => { if (OnProductPurchaseFinished != null) OnProductPurchaseFinished(new CallbackResponse<PurchaseResults> { Exception = ex, Status = CallbackStatus.Failure, Result = null }); });
                        return;
                    }

                    Utils.RunOnUnityAppThread(() => { if (OnProductPurchaseFinished != null) OnProductPurchaseFinished(new CallbackResponse<PurchaseResults> { Exception = null, Status = CallbackStatus.Success, Result = result }); });
                });
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
            DebugLog.Log(LogLevel.Info, "entered LoadLicenseXmlFile");

            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    licenseFilePath = (licenseFilePath == null) ? "WindowsStoreProxy.xml" : licenseFilePath;

                    StorageFile licenseFile = await Package.Current.InstalledLocation.GetFileAsync(licenseFilePath);
                    await CurrentAppSimulator.ReloadSimulatorAsync(licenseFile);

                    // switch on the license simulation
                    _isLicenseSimulationOn = true;

                    if (callback != null)
                    {
                        Utils.RunOnUnityAppThread( () => 
                        {
                            callback(new CallbackResponse { Exception = null, Status = CallbackStatus.Success });
                        });
                    }
                }
                catch (Exception ex)
                {
                    DebugLog.Log(LogLevel.Fatal, "Error loading license file. License simulator will give incorrect results!" + ex.ToString());
                    callback(new CallbackResponse { Exception = ex, Status = CallbackStatus.Failure });
                    return;
                }
            });
        }
    }
}
