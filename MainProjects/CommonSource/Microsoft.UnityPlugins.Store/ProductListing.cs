using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class ProductListing
    {
        public ProductListing(Windows.ApplicationModel.Store.ProductListing productListing)
        {
            FormattedPrice = productListing.FormattedPrice;
            Name = productListing.Name;
            ProductId = productListing.ProductId;
        }

        public string FormattedPrice { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
    }
}
