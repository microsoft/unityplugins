using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Microsoft.UnityPlugins;
using Assets.Scripts;

public class AzureMobileServiceDemoScript : MonoBehaviour
{
    public Button FacebookAuthButton;
    public Text FacebookAuthText;
    public string id = null;
    private Microsoft.UnityPlugins.MobileServiceUser serviceUser = null;

    public void OnFacebookAuthButtonClicked()
    {
        Debug.Log("In Facebook auth button clicked");
        AzureMobileServices.Connect("https://unitypluginstest.azure-mobile.net/", "cbdWXFvfYQsNLApFEnoRJnFoZzPWPS37");
        AzureMobileServices.AuthenticateWithServiceProvider(MobileServiceAuthenticationProvider.Facebook, (response) =>
        {
            if(response.Status == CallbackStatus.Failure)
            {
                Debug.LogError("AuthenticateWithServiceProvider failed.");
                Debug.LogError(response.Exception.ToString());
                return;
            }

            FacebookAuthText.text = "authenticating";
            serviceUser = response.Result;
            Debug.Log("Authentication Suceeded");
            FacebookAuthText.text = "authentication succeeded!";
        });
    }

    public void OnConnectButtonClicked()
    {
        AzureMobileServices.Connect("https://unitypluginstest.azure-mobile.net/", "cbdWXFvfYQsNLApFEnoRJnFoZzPWPS37");
    }

    public void OnInsertButtonClicked()
    {
        if(serviceUser == null)
        {
            FacebookAuthText.text = "Please authenticate before using futher scenarios!";
            return;
        }

        TodoItem item1 = new TodoItem { Complete = false, Text = "Just Inserted text!!" };
        AzureMobileServices.Insert<TodoItem>(item1, (response) =>
        {
            if (response.Status == CallbackStatus.Failure)
            {
                Debug.LogError("Inserting record failed.");
                Debug.LogError(response.Exception.ToString());
                return;
            }

            // print something
            FacebookAuthText.text = "Successfully inserted item";
            Debug.Log("Successfully inserted item");
            id = item1.Id;
        });
    }

    public void OnUpdateButtonClicked()
    {
        if (serviceUser == null)
        {
            FacebookAuthText.text = "Please authenticate before using futher scenarios!";
            return;
        }

        TodoItem item1 = new TodoItem { Complete = false, Text = "Todo Updated", Id = "5a34a00d8ef34630b6fe19e0e2c31b8b" };
        AzureMobileServices.Update<TodoItem>(item1, (response) =>
        {
            if (response.Status == CallbackStatus.Failure)
            {
                Debug.LogError("Updating record failed.");
                Debug.LogError(response.Exception.ToString());
                return;
            }


            // print something
            FacebookAuthText.text = "Successfully Updated item";
            Debug.Log("Successfully Updated item");
        });
    }

    public void OnDeleteButtonClicked()
    {
        if (serviceUser == null)
        {
            FacebookAuthText.text = "Please authenticate before using futher scenarios!";
            return;
        }

        TodoItem item1 = new TodoItem { Complete = false, Text = "Todo Updated", Id = this.id };
        AzureMobileServices.Delete<TodoItem>(item1, (response) =>
        {
            if (response.Status == CallbackStatus.Failure)
            {
                Debug.LogError("Deleting record failed.");
                Debug.LogError(response.Exception.ToString());
                return;
            }

            // print something
            FacebookAuthText.text = "Successfully deleted item";
            Debug.Log("Successfully deleted item");
        });
    }

    public void OnLookupButtonClicked()
    {
        if (serviceUser == null)
        {
            FacebookAuthText.text = "Please authenticate before using futher scenarios!";
            return;
        }

        string id = "5a34a00d8ef34630b6fe19e0e2c31b8b"; ;
        AzureMobileServices.Lookup<TodoItem>(id, (response) =>
        {
            if (response.Status == CallbackStatus.Failure)
            {
                Debug.LogError("Lookup record failed.");
                Debug.LogError(response.Exception.ToString());
                return;
            }

            // print something
            FacebookAuthText.text = "Successfully looked up item";
            Debug.Log("Successfully looked up item");
        });
    }

    public void OnWhereButtonClicked()
    {
        if (serviceUser == null)
        {
            FacebookAuthText.text = "Please authenticate before using futher scenarios!";
            return;
        }

        TodoItem item1 = new TodoItem { Complete = false, Text = "Todo Updated", Id = "5a34a00d8ef34630b6fe19e0e2c31b8b" };
        AzureMobileServices.Where<TodoItem>(item => item.Text.Contains("Updated"), (response) =>
        {
            if (response.Status == CallbackStatus.Failure)
            {
                Debug.LogError("Where record failed.");
                Debug.LogError(response.Exception.ToString());
                return;
            }


            // print something
            FacebookAuthText.text = "Successfully looked up item by where";
            Debug.Log("Successfully looked up item by where");
        });
    }
}
