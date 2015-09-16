using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class RoamingSettings
    {
        public static String RoamingFolder { get; set; }
        public static ulong RoamingStorageQuota { get; set; }

        public static event Action OnDataChanged;

        static RoamingSettings()
        {
        }

        public static string[] AllContainerNames
        {
            get
            {
                return new string[1];
            }
        }
        public static string[] AllKeys { get; }
        public static void ClearAllApplicationData(Action<CallbackResponse> OnClearAppDataFinished)
        {
            if(OnClearAppDataFinished != null)
            {
                OnClearAppDataFinished(new CallbackResponse { Status = CallbackStatus.Failure, Exception = new Exception("Cannot call Windows Store API in the Unity Editor") });
            }
        }


        public static void SetValueForKey(string key, object value)
        {
        }

        public static void SetValueForKeyInContainer(string containerName, string key, object value)
        {
        }

        public static void DeleteContainer(string containerName)
        {

        }

        public static void DeleteValueForKey(string key)
        {

        }

        public static void DeleteValueForKeyInContainer(string containerName, string key)
        {

        }

        public static object GetValueForKey(string key)
        {
            return GetValueForKeyInContainer(null, key, null);
        }

        public static object GetValueForKey(string key, object defaultValue)
        {
            return GetValueForKeyInContainer(null, key, defaultValue);
        }

        public static object GetValueForKeyInContainer(string key)
        {
            return GetValueForKeyInContainer(null, key, null);
        }

        public static object GetValueForKeyInContainer(string containerName, string key)
        {
            return GetValueForKeyInContainer(containerName, key, null);
        }

        public static object GetValueForKeyInContainer(string containerName, string key, object defaultValue)
        {
            return new object();
        }
    }
}

