---
layout: default
title: Store Plugin
---

##Introduction
Store Plugin (Microsoft.UnityPlugins.Store.unitypackage) contains APIs to easily integrate Windows Store related functionality into Unity based Windows Store apps.

##APIs
> Windows Store APIs can be invoked in two modes: Simulator Mode and against the real Windows Store. To invoke the APIs in Simulator mode, you need to place an XML file containing data that the license simulator will use. Example of such a file is present [here](https://github.com/Microsoft/unityplugins/blob/master/Samples/StoreTest/out_win10/StoreTest/WindowsStoreProxy.xml). To test an app against the real Windows Store, the app must be onboarded to the Windows Store. You may use the Hidden mode of the Windows Store in case you want to create a test app that you do not intend to release but only want to test against. Once an app has been published, you need to do the following:
>
> * Download the app from the store onto your device
> * Using Visual Studio options (Store->Associate app with store), associate the app with the store. This brings in the right metadata from store into your app.
> * Deploy your app on top of the app obtained from the store. At this point, you should be able to test the APIs against the real Windows Store. Make note that if you have paid In-App purchases, they will charge you real money.

### Enumerations
```C#
public enum FulfillmentResult
{
    Succeeded  = 0,
    NothingToFulfill = 1,
    PurchasePending = 2,
    PurchaseReverted = 3,
    ServerError = 4
}


public enum ProductPurchaseStatus
{
    Succeeded = 0,
    AlreadyPurchased = 1,
    NotFulfilled = 2,
    NotPurchased = 3
}

public enum ProductType
{
    Unknown = 0,
    Durable = 1,
    Consumable = 2
}

```

### Stub/proxy classes
```C#
public class LicenseInformation
{
    public DateTime ExpirationDate { get; set; }
    public Dictionary<string, ProductLicense> ProductLicenses { get; set; }
    public bool IsActive { get; set; }
    public bool IsTrial { get; set; }
}

public class ListingInformation
{
    public uint AgeRating { get; set; }
    public string CurrentMarket { get; set; }
    public string Description { get; set; }
    public string FormattedPrice { get; set; }
    public string Name { get; set; }
    public Dictionary<string, ProductListing> ProductListings { get; set; }
}

public class ProductLicense
{
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public string ProductId { get; set; }
}

public class ProductListing
{
        public string FormattedPrice { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
}	

public class PurchaseResults
{
    public string OfferId { get; set; }
    public string ReceiptXml { get; set; }
    public ProductPurchaseStatus Status { get; set;}
    // TODO: (sanjeevd) This was a GUID earlier
    public string TransactionId { get; set; }
}

public enum StatusCodeType
{
    Failure = 0,
    Success = 1
}
public class ReceiptResponse
{
    public string Description { get; set; }
    public StatusCodeType Status { get; set; }
    public string AppId;
    public List<Product> ProductIDs;
    public string ReceiptId;
    public bool IsValidReceipt;
    public bool IsBeta;
}

public class Product
{
    public string ReceiptId;
    public string ProductId;
}

public class UnfulfilledConsumable
{
    public string OfferId { get; set; }
    public string ProductId { get; set; }
    public Guid TransactionId { get; set; }

}
	
```

### Store class 

```
public static void RegisterForLicenseChangeEvents(Action OnLicenseChangedHandler);

public static bool IsInTrialMode();

public static void LoadListingInformation(
		Action<CallbackResponse<ListingInformation>> OnLoadListingFinished);

public static void VerifyReceipt(string receipt, 
		Action<CallbackResponse<ReceiptResponse>> OnReceiptVerified);

public static void VerifyReceipt(string receiptWebserviceUrl, 
		string receipt, 
		Action<CallbackResponse<ReceiptResponse>> OnReceiptVerified);

public static LicenseInformation GetLicenseInformation();

public static ProductLicense GetProductLicense(string productId);

public static void GetAppReceipt(Action<CallbackResponse<string>> OnAppReceiptAcquired);

public static void GetProductReceipt(string productId, 
		Action<CallbackResponse<string>> OnProductReceiptAcquired);

public static void LoadUnfulfilledConsumables(
		Action<CallbackResponse<List<UnfulfilledConsumable>>> OnLoadUnfulfilledConsumablesFinished);

public static void ReportConsumableFulfillment(string productId, Guid transactionId,
		Action<CallbackResponse<FulfillmentResult>> OnReportConsumableFulfillmentFinished);
	
public static void RequestAppPurchase(bool requireReceipt, 
		Action<CallbackResponse<string>> OnAppPurchaseFinished);

public static void RequestProductPurchase(string productId, 
		Action<CallbackResponse<PurchaseResults>> OnProductPurchaseFinished);

public static void LoadLicenseXMLFile(Action<CallbackResponse> callback, 
		string licenseFilePath = null);

```

## Sample
A sample is included in the github repository under Samples/StoreTest folder.