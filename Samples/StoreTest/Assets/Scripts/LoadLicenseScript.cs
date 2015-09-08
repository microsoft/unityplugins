using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Microsoft.UnityPlugins;

public class LoadLicenseScript : MonoBehaviour
{
    public Button loadLicenseXmlButton;
    public Text text;
    public void LoadLicenseXmlButton()
    {
        Debug.Log("LoadLicenseXmlButton clicked");
//#if UNITY_EDITOR
//            Debug.Log("Button Clicked");
//            text.text = "Button Clicked";
//#endif

        Store.LoadLicenseXMLFile((response) =>
        {
            Debug.Log("Button Clicked");
            text.text = "Windows Store Proxy xml file loaded successfully!";
        });
    }

    public void IsInTrialApp()
    {
        Debug.Log("Is In Trial button clicked");

        if (Store.IsInTrialMode())
        {
            text.text = "Yes! App is in trial mode";
        }
        else
        {
            text.text = "No! App is not in trial mode";
        }
    }

    public void LoadListingInformationForApp()
    {
        Debug.Log("Is In Trial button clicked");

        Store.LoadListingInformation((response) =>
        {
            text.text = "Finished Loading Listing Information";
            Debug.Log(response.Result.Description.ToString());
            foreach (var productListingKey in response.Result.ProductListings.Keys)
            {
                Debug.Log("Key: " + productListingKey + " value: " + response.Result.ProductListings[productListingKey].Name);
            }
        });
    }

    public void GetLicenseInformationForApp()
    {
        Debug.Log("Get License Information Button clicked");
        text.text = "Got License Information";
        var licenseInformation = Store.GetLicenseInformation();
        Debug.Log("LicenseInformation: IsActive " + licenseInformation.IsActive.ToString());
        Debug.Log("LicenseInformation: IsTrial " + licenseInformation.IsTrial.ToString());
        Debug.Log("LicenseInformation: ExpirationDate " + licenseInformation.ExpirationDate.ToString());
        foreach (var productLicense in licenseInformation.ProductLicenses.Keys)
        {
            Debug.Log("Key: " + productLicense + " value: " + licenseInformation.ProductLicenses[productLicense].ProductId);
        }
    }

    public void GetProductLicenseForApp()
    {
        Debug.Log("Get product license clicked");
        
        Store.GetProductLicense("Durable1");
        text.text = "Got product license";
    }

    public void GetAppReceiptForApp()
    {
        Debug.Log("Get app receipt");

        Store.GetAppReceipt((response) =>
        {
            text.text = "got app receipt";
            Debug.Log("App Receipt: " + response.Result);
        });
    }

    public void GetProductReceiptForApp()
    {
        Debug.Log("Get product license clicked");

        Store.GetProductReceipt("Consumable1", ((response) =>
        {
            text.text = "got product receipt";
            Debug.Log("Product Receipt: " + response.Result);
        }));
    }

    public void LoadUnfulfilledConsumablesForApp()
    {
        Store.LoadUnfulfilledConsumables((response) =>
        {
            Debug.Log("Loaded Unfulfilled consumables");
            foreach (var u in response.Result)
            {
                Debug.Log("offerId: " + u.OfferId + "transactionId: " + u.TransactionId);
            }
            text.text = "Loaded Unfulfilled consumables";
        });
    }

    public void ReportConsumablesFulfilledForApp()
    {
        System.Guid guid= new System.Guid("00000001-0000-0000-0000-000000000000");
        Store.ReportConsumableFulfillment("consumable1", guid, (response) =>
        {
            Debug.Log("Reported Consumable Fulfillment Result: " + response.Result.ToString());
            text.text = "Reported Consumable fulfillment Result: " + response.Result.ToString();
        });
    }

    public void RequestAppPurchaseForApp()
    {
        Debug.Log("RequestingAppPurchaseForApp begun");
        Store.RequestAppPurchase(true, (response) =>
        {
            if (response.Status == CallbackStatus.Success)
            {
                text.text = "Purchase Succeeded";
                if (!string.IsNullOrEmpty(response.Result))
                {
                    Debug.Log("App Purchase Receipt: " + response.Result);
                }
            }
            else
            {
                text.text = "Purchase failed";
            }
        });
    }

    public void RequestProductPurchaseForApp()
    {
        Debug.Log("RequestinProductpPurchaseForApp begun");
        Store.RequestProductPurchase("Consumable1", (response) =>
        {
            text.text = "Purchase status: " + response.Result.Status.ToString();
            Debug.Log("Purchase Status: " + response.Result.Status.ToString());
            Debug.Log("Purchase OfferId: " + response.Result.OfferId);
            Debug.Log("Purchase ReceiptXml: " + response.Result.ReceiptXml);

            Store.VerifyReceipt(response.Result.ReceiptXml, (ReceiptResponse) =>
            {
                Debug.Log("response: " + ReceiptResponse.Result.AppId);
                Debug.Log("response: " + ReceiptResponse.Result.IsValidReceipt.ToString());
            });
        });
    }
}
