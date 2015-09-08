using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class Utils
    {
        public static void Initialize(Action<Action> unityAppThreadInvokerDelegate) { }
        public static void RunOnUnityAppThread(Action delegateToExecute) { }
        public static void RunOnWindowsUIThread(Action delegateToExecute) { }
    }
}
