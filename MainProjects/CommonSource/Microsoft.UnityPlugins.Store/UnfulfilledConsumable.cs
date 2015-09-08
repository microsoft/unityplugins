using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class UnfulfilledConsumable
    {
        public UnfulfilledConsumable(Windows.ApplicationModel.Store.UnfulfilledConsumable unfulfilledConsumable)
        {
            OfferId = unfulfilledConsumable.OfferId;
            ProductId = unfulfilledConsumable.ProductId;
            TransactionId = unfulfilledConsumable.TransactionId;
        }

        public string OfferId { get; set; }
        public string ProductId { get; set; }
        public Guid TransactionId { get; set; }

    }
}
