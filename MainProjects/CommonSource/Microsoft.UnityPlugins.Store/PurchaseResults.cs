using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class PurchaseResults
    {
        public PurchaseResults(Windows.ApplicationModel.Store.PurchaseResults purchaseResults)
        {
            OfferId = purchaseResults.OfferId;
            ReceiptXml = purchaseResults.ReceiptXml;
            Status = (ProductPurchaseStatus)purchaseResults.Status;
            TransactionId = purchaseResults.TransactionId;
        }

        public string OfferId { get; set; }
        public string ReceiptXml { get; set; }
        public ProductPurchaseStatus Status { get; set;}
        
        public Guid TransactionId { get; set; }
    }

}
