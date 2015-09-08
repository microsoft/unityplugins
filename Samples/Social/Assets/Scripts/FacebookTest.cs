using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Microsoft.UnityPlugins;

public class FacebookTest : MonoBehaviour {

    public Button button;
    public Text text;

    public void TestFacebookInit()
    {
        Debug.Log("Initializting Facebook");
        text.text = "Facebook Initialized";
        FB.Init(() =>
        {
            Debug.Log("Init Completed");
        },
        "540541885996234", 
        (a) => { Debug.Log("OnHideUnity Done"); });
    }

    public void TestFBLogin()
    {
        Debug.Log("Entering Testing Facebook Login");
        
        FB.Login("user_profile", (results) =>
        {
            if (results != null)
            {
                Debug.Log("Logging Login Results!");
                Debug.Log(results.Json);
                Debug.Log(results.Text);
            }
            else
            {
                Debug.Log("Login hit an error!");
            }

            text.text = "Done with Login";
        });
    }

    public void TestFBLogout()
    {
        Debug.Log("Entering Testing Facebook Logout");
        FB.Logout();
        text.text = "Done with Logout";
        Debug.Log("Exiting Testing Facebook Logout");
    }

    public void TestFBAppRequests()
    {
        Debug.Log("Entering Testing Facebook App Requests");
        FB.AppRequest("This is a message");
        text.text = "Done with AppRequests";
        Debug.Log("Exiting Testing Facebook AppRequests");
    }

    public void TestFBFeedDialog()
    {
        Debug.Log("Entering Testing Facebook feed");
        FB.Feed();
        text.text = "Done with Feed Dialog";
        Debug.Log("Entering Testing Facebook feed");
    }

    public void TestFBAPI()
    {
        Debug.Log("Entering Testing Facebook API");
        FB.API("/me", HttpMethod.GET, (results) =>
        {
            if (results != null)
            {
                Debug.Log("Logging API results!");
                Debug.Log(results.Json);
                Debug.Log(results.Text);
            }
            else
            {
                Debug.Log("API hit an error!");
            }

            text.text = "Done with API Test";
        });
    }
}
