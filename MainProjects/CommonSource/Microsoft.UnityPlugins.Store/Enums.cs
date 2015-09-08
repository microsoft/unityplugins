using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{

    public enum FulfillmentResult
    {
        Succeeded = 0,
        NothingToFulfill = 1,
        PurchasePending = 2,
        PurchaseReverted = 3,
        ServerError = 4
    }


    public enum ProductPurchaseStatus
    {
        Succeeded = 0,
        AlreadyPurchased = 1,
        NotFulfilled = 2,
        NotPurchased = 3
    }

    public enum ProductType
    {
        Unknown = 0,
        Durable = 1,
        Consumable = 2
    }
}
