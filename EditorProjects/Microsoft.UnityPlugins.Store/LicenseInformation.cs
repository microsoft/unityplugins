using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class LicenseInformation
    {
        public DateTime ExpirationDate { get; set; }
        public Dictionary<string, ProductLicense> ProductLicenses { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrial { get; set; }
    }
}
