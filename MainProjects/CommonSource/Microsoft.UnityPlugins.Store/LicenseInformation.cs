using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class LicenseInformation
    {
        public LicenseInformation(Windows.ApplicationModel.Store.LicenseInformation licenseInformation)
        {
            ExpirationDate = licenseInformation.ExpirationDate.DateTime;
            
            IsActive = licenseInformation.IsActive;
            IsTrial = licenseInformation.IsTrial;

            ProductLicenses = new Dictionary<string, ProductLicense>();
            foreach (var key in licenseInformation.ProductLicenses.Keys)
            {
                ProductLicenses.Add(key, new ProductLicense(licenseInformation.ProductLicenses[key]));
            }
        }

        public DateTime ExpirationDate { get; set; }
        public Dictionary<string, ProductLicense> ProductLicenses { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrial { get; set; }
    }
}
