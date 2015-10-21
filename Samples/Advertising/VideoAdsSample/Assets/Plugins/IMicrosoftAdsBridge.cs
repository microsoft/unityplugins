using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

namespace Microsoft.UnityPlugins
{
    public enum InterstitialAdState
    {
        NotReady = 0,
        Ready = 1,
        Showing = 2,
        Closed = 3
    }

    public enum AdType
    {
        Video = 0
    }

    public enum AdCallback
    {
        Ready,
        Completed,
        Error,
        Cancelled

    }

    //
    // Summary:
    //     An enumeration of error codes that identify various reasons ads could not be
    //     downloaded.  
    public enum ErrorCode
    {
        //
        // Summary:
        //     Default value. Cause of error is unknown.
        Unknown = 0,
        //
        // Summary:
        //     The server was unable to find an ad that matched the request.
        NoAdAvailable = 1,
        //
        // Summary:
        //     An error occurred when making a network request.
        NetworkConnectionFailure = 2,
        //
        // Summary:
        //     One of the required parameters set by the app developer is not valid or not set.
        ClientConfiguration = 3,
        //
        // Summary:
        //     An error occurred on the server while handling the ad request.
        ServerSideError = 4,
        //
        // Summary:
        //     The server returned a response that could not be parsed or contained ad data
        //     that was invalid.
        InvalidServerResponse = 5,
        //
        // Summary:
        //     The error code for errors not covered by other codes.
        Other = 6,
        //
        // Summary:
        //     Refresh was attempted but not allowed due to the current state of the ad control
        //     (e.g. not visible on screen).
        RefreshNotAllowed = 7,
        //
        // Summary:
        //     The ad creative experienced an error.
        CreativeError = 8,
        //
        // Summary:
        //     An error occurred while performing an MRAID operation triggered by the ad.
        MraidOperationFailure = 9,
        //
        // Summary:
        //     Action succeeded.
        Success = 10,
        //
        // Summary:
        //     Action was cancelled.
        Cancelled = 11,
        //
        // Summary:
        //     Error occurred while performing a file operation.
        FileOperationFailure = 12,
        //
        // Summary:
        //     Error occurred while extracting data from the ad payload.
        ParseToBOMFailure = 13,
        //
        // Summary:
        //     Error occurred during validation of the ad payload.
        ValidationFailure = 14
    }

    public class AdErrorEventArgs 
    {
        public ErrorCode ErrorCode { get; private set;  }
        public string ErrorMessage { get; private set;  }

        public AdErrorEventArgs(ErrorCode code, string message)
        {
            ErrorCode = code;
            ErrorMessage = message; 
        }
    }

    public interface IInterstittialAd : IDisposable
    {

        void AddCallback(AdCallback type, Action<object> cb);
        void ClearCallback(AdCallback type);

        void RequestAndShow(string appId, string adUnitId);

        void Request(string appId, string adUnitId, AdType type);

        void Show();
        InterstitialAdState State { get; }
    }

    public interface IInterstitialAdFactory
    {
        IInterstittialAd CreateAd();

        IInterstittialAd CreateAd( Action<object> readyCallback, Action<object> completedCallback, Action<object> cancelledCallback,
            Action<object> errorCallback );
    }

    public class MicrosoftAdsBridge
    {
        public static IInterstitialAdFactory InterstitialAdFactory { get; set; }
    }

} 


