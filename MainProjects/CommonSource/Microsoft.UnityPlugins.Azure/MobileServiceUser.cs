using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;

namespace Microsoft.UnityPlugins
{
    public class MobileServiceUser
    {
        public string MobileServiceAuthenticationToken { get; set; }
        public string UserId { get; set; }
        public MobileServiceUser(Microsoft.WindowsAzure.MobileServices.MobileServiceUser mobileServiceUser)
        {
            this.MobileServiceAuthenticationToken = mobileServiceUser.MobileServiceAuthenticationToken;
            this.UserId = mobileServiceUser.UserId;
        }
        
    }
}
