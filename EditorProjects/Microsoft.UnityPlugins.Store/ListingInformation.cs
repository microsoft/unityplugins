using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class ListingInformation
    {
        public uint AgeRating { get; set; }
        public string CurrentMarket { get; set; }
        public string Description { get; set; }
        public string FormattedPrice { get; set; }
        public string Name { get; set; }
        public Dictionary<string, ProductListing> ProductListings { get; set; }
    }
}
