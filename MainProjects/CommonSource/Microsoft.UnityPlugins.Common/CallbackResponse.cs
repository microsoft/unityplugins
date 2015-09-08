using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{
    public enum CallbackReponseType
    {
        ResponsNotDesired = 0,
        ResponseDesired = 1
    }

    public enum CallbackStatus
    {
        Unknown = -1,
        Failure = 0,
        Success = 1,
        TimedOut = 2
    }

    public class CallbackResponse
    {
        public CallbackStatus Status { get; set; }
        public Exception Exception { get; set; }
    }

    public class CallbackResponse<T> : CallbackResponse
    {
        public T Result { get; set; }
    }
}
