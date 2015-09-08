using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Microsoft.UnityPlugins
{
    public class Utils
    {

        /// <summary>
        /// When initializing the XAML app, we will set this variable up to invoke functions on the unity thread.
        /// The funky look of Action<Action> simply means that it is a parameterized delegate which takes another
        /// delegate as a parameter and executes it within itself. 
        /// </summary>
        private static Action<Action> _unityAppThreadInvokerDelegate;

        /// <summary>
        /// This function must be called from the InitializeUnity function at the end to ensure that
        /// we setup the Windows and Unity delegates to invoke various plugin actions with the Windows
        /// or Unity threads
        /// </summary>
        /// <param name="unityAppThreadInvokerDelegate"></param>
        public static void Initialize(Action<Action> unityAppThreadInvokerDelegate)
        {
            _unityAppThreadInvokerDelegate = unityAppThreadInvokerDelegate;
        }

        //public delegate void SetInputFocusOnUnity(bool enabled);
        //public static SetInputFocusOnUnity handler;

        /// <summary>
        /// A bit of primer on how executing delegates on different threads works. When one thread wants to execute something 
        /// on the other thread, it takes the work item and inserts it in the "dispatch" queue for the other thread. The other
        /// thread picks whatever is in its work queue and runs/dispatches it. So, we can keep bouncing between threads as much
        /// as we want.
        /// </summary>
        /// <param name="delegateToExecute"></param>
        public static void RunOnUnityAppThread(Action delegateToExecute)
        {
            if (delegateToExecute == null)
            {
                throw new ArgumentNullException("delegateToExecute", "You must pass a non-null delegate to execute on the Unity thread when invoking RunOnUnityAppThread");
            }

            try
            {
                _unityAppThreadInvokerDelegate(delegateToExecute);
            }
            catch (Exception ex)
            {
                DebugLog.Log(LogLevel.Error, "Error running task on the Unity thread " + ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateToExecute"></param>
        public static void RunOnWindowsUIThread(Action delegateToExecute)
        {
            if (delegateToExecute == null)
            {
                throw new ArgumentNullException("delegateToExecute", "You must pass a non-null delegate to execute on the Unity thread when invoking RunOnWindowsUIThread");
            }

            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                try
                {
                    delegateToExecute(); // run the action now
                }
                catch (Exception ex)
                {
                    DebugLog.Log(LogLevel.Error, "Error running task on the Windows UI thread " + ex.ToString());
                }
            });
        }
    }
}
