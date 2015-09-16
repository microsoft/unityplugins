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
    public class AzureMobileServices
    {
        
        private static Microsoft.WindowsAzure.MobileServices.MobileServiceClient mobileServiceClient = null;
        private static Microsoft.WindowsAzure.MobileServices.MobileServiceUser user = null;

        public static void AuthenticateWithServiceProvider(MobileServiceAuthenticationProvider authenticationProvider, 
            Action<CallbackResponse<MobileServiceUser>> OnAuthenticationFinished)
        {
            Task.Run(() =>
            {
                Utils.RunOnWindowsUIThread(async () =>
                {
                    try
                    {
                        user = await
                            mobileServiceClient.LoginAsync(
                                Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Facebook);
                    }
                    catch (Exception ex)
                    {
                        if (OnAuthenticationFinished != null)
                        {
                            Utils.RunOnUnityAppThread(() =>
                            {

                                OnAuthenticationFinished(new CallbackResponse<MobileServiceUser> { Result = null, Status = CallbackStatus.Failure, Exception = ex });

                            });
                        }
                        return;
                    }

                    if (OnAuthenticationFinished != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                        {
                            OnAuthenticationFinished(new CallbackResponse<MobileServiceUser> { Result = new Microsoft.UnityPlugins.MobileServiceUser(user), Status = CallbackStatus.Success, Exception = null });
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

        public static void Insert<T>(T item, Action<CallbackResponse> OnInsertFinished)
        {
            Task.Run(() =>
            {
                Utils.RunOnWindowsUIThread(async () =>
                {
                    try
                    {
                        await mobileServiceClient.GetTable<T>().InsertAsync(item);
                    }
                    catch(Exception ex)
                    {
                        if (OnInsertFinished != null)
                        {
                            Utils.RunOnUnityAppThread(() =>
                            {

                                OnInsertFinished(new CallbackResponse {Status = CallbackStatus.Failure, Exception = ex });

                            });
                        }
                        return;
                    }

                    if (OnInsertFinished != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                        {
                            OnInsertFinished(new CallbackResponse { Status = CallbackStatus.Success, Exception = null});
                        });
                    }
                });
            });
        }

        public static void Lookup<T>(String id, Action<CallbackResponse<T>> OnLookupFinished)
        {
            Task.Run(async () =>
            {
                try
                {
                    T itemFound = await mobileServiceClient.GetTable<T>().LookupAsync(id);
                    if (itemFound != null)
                    {
                        if (OnLookupFinished != null)
                        {
                            Utils.RunOnUnityAppThread(() =>
                            {
                                OnLookupFinished(new CallbackResponse<T> {Result = itemFound, Status = CallbackStatus.Success, Exception = null });
                            });
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (OnLookupFinished != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                        {
                            OnLookupFinished(new CallbackResponse<T> {Result = default(T), Status = CallbackStatus.Failure, Exception = ex });
                        });
                    }
                    return;
                }
            });
        }


        public static void Update<T>(T item, Action<CallbackResponse> OnUpdateFinished)
        {
            Task.Run(async () =>
            {
                try
                {
                    await mobileServiceClient.GetTable<T>().UpdateAsync(item);

                }
                catch(Exception ex)
                {
                    OnUpdateFinished(new CallbackResponse { Status = CallbackStatus.Failure, Exception = ex });
                    return;
                }

                if (OnUpdateFinished != null)
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        OnUpdateFinished(new CallbackResponse { Status = CallbackStatus.Success, Exception = null});
                    });
                }

            });
        }


        public static void Delete<T>(T item, Action<CallbackResponse> OnDeleteFinished)
        {
            Task.Run(async () =>
            {
                try
                {
                    await mobileServiceClient.GetTable<T>().DeleteAsync(item);
                }
                catch(Exception ex)
                {
                    OnDeleteFinished(new CallbackResponse { Status = CallbackStatus.Failure, Exception = ex });
                }

                if (OnDeleteFinished != null)
                {
                    Utils.RunOnUnityAppThread(() =>
                    {
                        OnDeleteFinished(new CallbackResponse { Status = CallbackStatus.Success, Exception = null });
                    });
                }
            });
        }

        public static void Where<T>(Expression<Func<T, bool>> predicate, Action<CallbackResponse<List<T>>> OnWhereFinished)
        {
            Task.Run(async () =>
            {
                try
                {
                    var query = mobileServiceClient.GetTable<T>().Where(predicate);
                    var queryResult = await query.ToListAsync();

                    if (OnWhereFinished != null)
                    {
                        Utils.RunOnUnityAppThread(() =>
                       {
                           OnWhereFinished(new CallbackResponse<List<T>> { Result = queryResult,  Status = CallbackStatus.Success, Exception = null});
                       });
                    }
                }
                catch(Exception ex)
                {
                    if (OnWhereFinished != null)
                    {
                        OnWhereFinished(new CallbackResponse<List<T>> { Result = null, Status = CallbackStatus.Failure, Exception = ex });
                    }
                }
            });
        }
    }
}
