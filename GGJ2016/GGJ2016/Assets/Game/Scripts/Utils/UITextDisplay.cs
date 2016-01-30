using UnityEngine;
using System.Collections;
using Common.Signal;
using UnityEngine.UI;

namespace Game
{
    public class UITextDisplay : MonoBehaviour
    {
        public Text textDisplay;

        void Awake()
        {
            GameSignals.DEBUG_LOG.AddListener(OnLogReceived);
        }

        void OnDestroy()
        {
            GameSignals.DEBUG_LOG.RemoveListener(OnLogReceived);
        }

        void OnLogReceived(ISignalParameters parameters)
        {
            var text = (string)parameters.GetParameter(GameParams.DEBUG_TEXT);
            textDisplay.text += text + "\n";
        }
    }
}