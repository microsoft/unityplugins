using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{
    public class Advertising
    {
        static InterstitialAd interstitialAd;
        static DateTime lastPlayed = DateTime.MinValue;
        static bool isShowRequested;
        static bool isAdRequested;
        static string adAppId;
        static string adUnitId;

        /// <summary>
        /// Initialize the interstitial ad and request the first ad
        /// </summary>
        /// <param name="appId">Microsoft Advertising application Id (set up in pubcenter)</param>
        /// <param name="unitId">Microsoft Advertising interstitial video ad id (set up in pubcenter)</param>
        public static void Init(string appId, string unitId)
        {
            Utils.RunOnWindowsUIThread(() =>
            {
                DebugLog.Log(LogLevel.Info, "Initializing interstitial ads...");
                interstitialAd = new InterstitialAd();
                interstitialAd.AdReady += InterstitialAd_AdReady;
                interstitialAd.ErrorOccurred += InterstitialAd_ErrorOccurred;
                interstitialAd.Completed += InterstitialAd_Completed;
                interstitialAd.Cancelled += InterstitialAd_Cancelled;

                if (interstitialAd != null)
                {
                    adAppId = appId;
                    adUnitId = unitId;
                    // automatically request the first ad
                    DebugLog.Log(LogLevel.Info, "Requesting interstitial ads...");
                    interstitialAd.RequestAd(AdType.Video, adAppId, adUnitId);
                    isAdRequested = true;
                }
            });
            
        }

        /// <summary>
        /// You can call show, when the ad is ready it will be started automatically
        /// Interstitial documentation recommends at least 60 seconds between ads
        /// </summary>
        public static void Show()
        {
            DebugLog.Log(LogLevel.Info, "Show");
            Utils.RunOnWindowsUIThread(() =>
            {
                if (interstitialAd.State == InterstitialAdState.Ready)
                {
                    isShowRequested = false;
                    try
                    {
                        interstitialAd.Show();
                    }
                    catch(Exception x) {
                        DebugLog.Log(LogLevel.Error, x.ToString());
                    }
                }
                else
                {
                    isShowRequested = true;
                }
            });
        }

        private static void InterstitialAd_Cancelled(object sender, object e)
        {
            DebugLog.Log(LogLevel.Info, "Cancelled");
            isShowRequested = false;
        }

        private static void InterstitialAd_Completed(object sender, object e)
        {
            DebugLog.Log(LogLevel.Info, "Completed");
            // request the next ad
            lastPlayed = DateTime.Now;
            Utils.RunOnWindowsUIThread(() =>
            {
                try
                {
                    interstitialAd.RequestAd(AdType.Video, adAppId, adUnitId);
                }
                catch (Exception x)
                {
                    DebugLog.Log(LogLevel.Error, x.ToString());
                }
            });
            isShowRequested = false;
        }

        private static void InterstitialAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            DebugLog.Log(LogLevel.Info, "ErrorOccurred " + e.ErrorMessage);
            isShowRequested = false;
        }

        private static void InterstitialAd_AdReady(object sender, object e)
        {
            DebugLog.Log(LogLevel.Info, "AdReady");
            // wait until canPlay == true
            Utils.RunOnWindowsUIThread(() =>
            {
                if (interstitialAd.State == InterstitialAdState.Ready && isShowRequested)
                {
                    isShowRequested = false;
                    try
                    {
                        interstitialAd.Show();
                    }
                    catch(Exception x) {
                        DebugLog.Log(LogLevel.Error, x.ToString());
                    }
                }
            });
        }

        
    }
}
