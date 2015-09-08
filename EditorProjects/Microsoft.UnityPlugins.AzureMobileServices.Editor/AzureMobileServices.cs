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
            Action<MobileServiceUser> OnAuthenticationFinished)
        {
            if(OnAuthenticationFinished != null)
            {
                OnAuthenticationFinished(new MobileServiceUser());
            }
        }

        public static void Connect(string applicationUri, string applicationKey)
        {
        }

        public static void Insert<T>(T item, Action OnInsertFinished)
        {
            if(OnInsertFinished != null)
            {
                OnInsertFinished();
            }
        }

        public static void Lookup<T>(string id, Action<T> OnLookupFinished)
        {
            if (OnLookupFinished != null)
            {
                OnLookupFinished(default(T));
            }
        }


        public static void Update<T>(T item, Action OnUpdateFinished)
        {
            if (OnUpdateFinished != null)
            {
                OnUpdateFinished();
            }
        }


        public static void Delete<T>(T item, Action OnDeleteFinished)
        {
            if (OnDeleteFinished != null)
            {
                OnDeleteFinished();
            }
        }

        public static void Where<T>(Expression<Func<T, bool>> predicate, Action<List<T>> OnWhereFinished)
        {
            if (OnWhereFinished != null)
            {
                OnWhereFinished(new List<T>());
            }
        }
    }
}
