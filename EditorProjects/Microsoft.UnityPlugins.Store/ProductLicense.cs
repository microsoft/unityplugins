using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class ProductLicense
    {
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public string ProductId { get; set; }
    }
}
