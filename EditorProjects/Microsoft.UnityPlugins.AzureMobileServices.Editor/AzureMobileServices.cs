using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Microsoft.UnityPlugins
{
    public class AzureMobileServices
    {
        public static void AuthenticateWithServiceProvider(MobileServiceAuthenticationProvider authenticationProvider,
            Action<CallbackResponse<MobileServiceUser>> OnAuthenticationFinished)
        {
            if(OnAuthenticationFinished != null)
            {
                OnAuthenticationFinished(new CallbackResponse<MobileServiceUser> { Result = null, Status = CallbackStatus.Failure, Exception = new Exception("Cannot invoke Windows Store API in the Editor") });
            }
        }

        public static void Connect(string applicationUri, string applicationKey)
        {
        }

        public static void Insert<T>(T item, Action<CallbackResponse> OnInsertFinished)
        {
            if(OnInsertFinished != null)
            {
                OnInsertFinished(new CallbackResponse { Status = CallbackStatus.Failure, Exception = new Exception("Cannot invoke Windows Store API in the Editor") });
            }
        }

        public static void Lookup<T>(String id, Action<CallbackResponse<T>> OnLookupFinished)
        {
            if (OnLookupFinished != null)
            {
                OnLookupFinished(new CallbackResponse<T> { Result = default(T), Status = CallbackStatus.Success, Exception = new Exception("Cannot invoke Windows Store API in the Editor") });
            }
        }


        public static void Update<T>(T item, Action<CallbackResponse> OnUpdateFinished)
        {
            if (OnUpdateFinished != null)
            {
                
                OnUpdateFinished(new CallbackResponse { Status = CallbackStatus.Failure, Exception = new Exception("Cannot invoke Windows Store API in the Editor") });
            }
        }


        public static void Delete<T>(T item, Action<CallbackResponse> OnDeleteFinished)
        {
            if (OnDeleteFinished != null)
            {
                OnDeleteFinished(new CallbackResponse { Status = CallbackStatus.Failure, Exception = new Exception("Cannot invoke Windows Store API in the Editor") });
            }
        }

        public static void Where<T>(Expression<Func<T, bool>> predicate, Action<CallbackResponse<List<T>>> OnWhereFinished)
        {
            if (OnWhereFinished != null)
            {
                OnWhereFinished(new CallbackResponse<List<T>> { Result = null, Status = CallbackStatus.Failure, Exception = new Exception("Cannot invoke Windows Store API in the Editor") });
            }
        }
    }
}
