using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{
    public enum StatusCodeType
    {
        Failure = 0,
        Success = 1
    }
    public class ReceiptResponse
    {
        public string Description { get; set; }
        public StatusCodeType Status { get; set; }
        public string AppId;
        public List<Product> ProductIDs;
        public string ReceiptId;
        public bool IsValidReceipt;
        public bool IsBeta;
    }

    public class Product
    {
        public string ReceiptId;
        public string ProductId;
    }
}
