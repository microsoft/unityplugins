using UnityEngine;
using System.Collections;
using System.Net;
using Microsoft.UnityPlugins;
using UnityEngine.UI;
using UnityEngineInternal;

public class mainSceneManager : MonoBehaviour {

	// Use this for initialization
    private UnityEngine.UI.Text status;
    private UnityEngine.UI.Toggle generateError;

    private IInterstittialAd ad; 

	void Start ()
	{
	    generateError = GameObject.Find("GenerateErrorToggle").GetComponent<Toggle>();
	    status = GameObject.Find("Status").GetComponent<Text>();
	    Debug.Assert(status != null && generateError != null);

#if UNITY_EDITOR
        //STEP1:  In your real project this should happen in App.xaml.cs and you will instantiata different factory.. 
	    Microsoft.UnityPlugins.MicrosoftAdsBridge.InterstitialAdFactory = new EditorAdFactory();       
#else 
        generateError.interactable = false;
        generateError.transform.FindChild("Label").GetComponent<Text>().color = new Color(0, 0, 0, 0);
#endif

        ad = MicrosoftAdsBridge.InterstitialAdFactory.CreateAd(OnAdReady, OnAdCompleted, OnAdCancelled, OnAdError);

#if UNITY_EDITOR
        //This is not needed in production.. here we use it to show a fake ad so you can work your 
        // workflow of stopping pausing to play ad, unpausing, etc.. 
	    ((EditorInterstitialAd)ad).syncMonoBehaviour = this;

	    var canvas = GameObject.Find("Canvas").GetComponent<Canvas>(); 
        ((EditorInterstitialAd)ad).canvas = canvas ; 
        
#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowAd()
    {
#if UNITY_EDITOR
        ((EditorInterstitialAd) ad).fireError = generateError.isOn;
#endif
        status.text = "showing..."; // set the status prior so that if there is an error the error comes later 
        ad.Show();
        
    }

    public void RequestAd()
    {
#if UNITY_EDITOR
        ((EditorInterstitialAd)ad).fireError = generateError.isOn;
#endif 
        // set the status prior so that if there is an error the error comes later 
        status.text = "requesting...";
        ad.Request( appId, nextAd , AdType.Video);
        
    }

    private const string appId = "d25517cb-12d4-4699-8bdc-52040c712cab";
    private string adUnitId = "11389925";

    protected string nextAd
    {
        get
        {
            
            string [] adUnitIds = new string[] { "11389925", "11389925", "0" };
            return adUnitIds[count%adUnitIds.Length]; 
        }            
    }

    


    private static int count = 0; 

    void OnAdReady( object unused )
    {
        status.text = ad.State.ToString(); 
    }

    void OnAdCompleted (object unused)
    {
        status.text = ad.State.ToString(); 
    }
    void OnAdCancelled (object unused)
    {
        status.text = ad.State.ToString(); 
    }
    void OnAdError (object param )
    { 
        AdErrorEventArgs args = param as AdErrorEventArgs;
        if  (args != null )
            status.text = "Error: " + args.ErrorMessage;
         
    }
}
