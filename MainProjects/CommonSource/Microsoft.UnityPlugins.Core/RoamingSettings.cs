using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Microsoft.UnityPlugins
{
    public class RoamingSettings
    {
        public static String RoamingFolder { get; set; }
        public static ulong RoamingStorageQuota { get; set; }

        public static event Action OnDataChanged;

        static RoamingSettings()
        {
            Windows.Storage.ApplicationData.Current.DataChanged += Current_DataChanged;
        }

        private static void Current_DataChanged(ApplicationData sender, object args)
        {
            if(OnDataChanged != null)
            {
                // TODO: (sanjeevd) This callback should be passed some context.. otherwise the user will have no idea
                // on what changed and what to do.
                OnDataChanged();
            }
        }

        public static string[] AllContainerNames
        {
            get
            {
                var containers =  Windows.Storage.ApplicationData.Current.RoamingSettings.Containers;
                string[] containerArray = new string[containers.Count];
                int count = 0;
                foreach(var container in containers)
                {
                    containerArray[count] = container.Value.Name;
                }

                return containerArray;
            }
        }
        public static string[] AllKeys { get; }
        public static void ClearAllApplicationData(Action OnClearAppDataFinished)
        {
            // TODO: (sanjeevd) This is hitting a FileLoadException... investigation needed
          Utils.RunOnWindowsUIThread(async () =>
           {
              await Windows.Storage.ApplicationData.Current.ClearAsync();

               if(OnClearAppDataFinished != null)
               {
                   Utils.RunOnUnityAppThread(() =>
                  {
                      OnClearAppDataFinished();
                  });
               }
           });
        }


        public static void SetValueForKey(string key, object value)
        {
            Windows.Storage.ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

        public static void SetValueForKeyInContainer(string containerName, string key, object value)
        {
            var container = Windows.Storage.ApplicationData.Current.RoamingSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
            container.Values[key] = value;
        }

        public static void DeleteContainer(string containerName)
        {
            Utils.RunOnWindowsUIThread(() =>
           {
               Windows.Storage.ApplicationData.Current.RoamingSettings.DeleteContainer(containerName);
           });
        }

        public static void DeleteValueForKey(string key)
        {
            if (Windows.Storage.ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values.Remove(key);
            }
        }

        public static void DeleteValueForKeyInContainer(string containerName, string key)
        {
            if (Windows.Storage.ApplicationData.Current.RoamingSettings.Containers.ContainsKey(containerName)
                && Windows.Storage.ApplicationData.Current.RoamingSettings.Containers[containerName].Values.ContainsKey(key))
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Containers[containerName].Values.Remove(key);
            }
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
            if(key == null)
            {
                throw new ArgumentNullException("key");
            }

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (containerName == null)
            {
                return roamingSettings.Values[key];
            }

            
            if(roamingSettings.Containers.ContainsKey(containerName))
            {
                var container = roamingSettings.Containers[containerName];
                if(container.Values.ContainsKey(key))
                {
                    return container.Values[key];
                }
            }

            return defaultValue;
        }
    }
}

