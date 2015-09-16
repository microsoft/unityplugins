using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Microsoft.UnityPlugins
{
    public class Speech
    {
        public static void ListenForCommands(IEnumerable<string> commands, Action<CallbackResponse<SpeechArguments>> OnSpeechResults)
        {
            if (OnSpeechResults != null)
            {
                OnSpeechResults(new CallbackResponse<SpeechArguments> { Result = null, Exception = new Exception("Cannot call Windows Store API in the Unity Editor"), Status = CallbackStatus.Failure});
            }
        }

        public static void ListenForDictation(Action<CallbackResponse<SpeechArguments>> OnSpeechResults)
        {
            if (OnSpeechResults != null)
            {
                OnSpeechResults(new CallbackResponse<SpeechArguments> { Result = null, Status = CallbackStatus.Failure, Exception = new Exception("Cannot call Windows Store API in the Unity Editor") });
            }
        }

        public static void Stop()
        {
        }
    }
}
