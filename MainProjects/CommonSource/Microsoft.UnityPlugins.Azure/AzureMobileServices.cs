using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Microsoft.UnityPlugins
{
    // Note: (sanjeevd) Azure Mobile Services PCL nuget does not have the LoginAsync with just 1 paramater which is used to
    // perform the authentication client side. So, I hacked it by removing the "Windows Phone 8.1" support, removed the reference,
    // reinstalled the nuget and then finally added the Windows Phone 8.1 target again. VS complained and mentioned that I should redo
    // the nuget intall but I ignored it. I am only planning for the plugin to be present only on Windows 8.1 .NET CORE and
    // Windows Phone 8.1 .NET CORE so I believe this should be fine.
    public class AzureMobileServices
    {
        
        private static Microsoft.WindowsAzure.MobileServices.MobileServiceClient mobileServiceClient = null;
        private static Microsoft.WindowsAzure.MobileServices.MobileServiceUser user = null;

        public static void AuthenticateWithServiceProvider(MobileServiceAuthenticationProvider authenticationProvider, 
            Action<MobileServiceUser> OnAuthenticationFinished)
        {
            Task.Run(() =>
            {
                Utils.RunOnWindowsUIThread(async () =>
                {
                    user = await
                        mobileServiceClient.LoginAsync(
                            Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Facebook);

                    if (OnAuthenticationFinished != null)
                    {
                        Utils.RunOnUnityAppThread(
                        () =>
                        {
                            OnAuthenticationFinished(new Microsoft.UnityPlugins.MobileServiceUser(user));

                        });
                    }
                });
            });
        }

        private static bool mobileServiceClientInitialized = false;
        private static object syncLock = new object();
        public static void Connect(string applicationUri, string applicationKey)
        {
            // the initialization should be done only once really, so locking the whole initial connection effort
            lock (syncLock)
            {
                if (mobileServiceClientInitialized != true)
                {
                    mobileServiceClient = new MobileServiceClient(applicationUri, applicationKey);
                    mobileServiceClientInitialized = true;
                }
            }
        }

        public static void Insert<T>(T item, Action OnInsertFinished)
        {
            Task.Run(() =>
            {
                Utils.RunOnWindowsUIThread(async () =>
                {
                    await mobileServiceClient.GetTable<T>().InsertAsync(item);

                    if (OnInsertFinished != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                        {
                            OnInsertFinished();
                        }
                        );
                    }
                });
            });
        }

        public static void Lookup<T>(String id, Action<T> OnLookupFinished)
        {
            Task.Run(async () =>
            {
                T itemFound = await mobileServiceClient.GetTable<T>().LookupAsync(id);

                if (itemFound != null)
                {
                    if (OnLookupFinished != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                        {
                            OnLookupFinished(itemFound);
                        });
                    }
                }
            });
        }


        public static void Update<T>(T item, Action OnUpdateFinished)
        {
            Task.Run(async () =>
            {
                await mobileServiceClient.GetTable<T>().UpdateAsync(item);

                if (OnUpdateFinished != null)
                {
                   Utils.RunOnUnityAppThread(() =>
                   {
                       OnUpdateFinished();
                   });
                }
            });
        }


        public static void Delete<T>(T item, Action OnDeleteFinished)
        {
            Task.Run(async () =>
            {
                await mobileServiceClient.GetTable<T>().DeleteAsync(item);

                if (OnDeleteFinished != null)
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        OnDeleteFinished();
                    });
                }
            });
        }

        public static void Where<T>(Expression<Func<T, bool>> predicate, Action<List<T>> OnWhereFinished)
        {
            Task.Run(async () =>
            {
                var query = mobileServiceClient.GetTable<T>().Where(predicate);
                var queryResult = await query.ToListAsync();

                if (OnWhereFinished != null)
                {
                    Utils.RunOnUnityAppThread(() =>
                   {
                       OnWhereFinished(queryResult);
                   });
                }
            });
        }
    }
}
