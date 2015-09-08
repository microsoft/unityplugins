using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Microsoft.UnityPlugins
{
    public class Speech
    {
        public static void ListenForCommands(IEnumerable<string> commands, Action<SpeechArguments> OnSpeechResults)
        {
            if (OnSpeechResults != null)
            {
                OnSpeechResults(new SpeechArguments());
            }
        }

        public static void ListenForDictation(Action<SpeechArguments> OnSpeechResults)
        {
            if (OnSpeechResults != null)
            {
                OnSpeechResults(new SpeechArguments());
            }
        }

        public static void Stop()
        {
        }
    }
}
