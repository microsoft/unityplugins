using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class PurchaseResults
    {
        public string OfferId { get; set; }
        public string ReceiptXml { get; set; }
        public ProductPurchaseStatus Status { get; set;}
        // TODO: (sanjeevd) This was a GUID earlier
        public string TransactionId { get; set; }
    }

}
