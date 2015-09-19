using UnityEngine;
using System.Collections;
using Microsoft.UnityPlugins;

public class AdvertisingDemo : MonoBehaviour {

    public void StartInterstitialAd()
    {
        Advertising.Init("d25517cb-12d4-4699-8bdc-52040c712cab", "11389925");
        Advertising.Show();
    }
}
