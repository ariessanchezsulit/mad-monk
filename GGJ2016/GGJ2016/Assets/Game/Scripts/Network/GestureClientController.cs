using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Common.Signal;

namespace Game
{
    public class GestureClientController : NetworkBehaviour
    {

        void Awake()
        {
            GameSignals.INPUT_GENERIC.AddListener(OnInputReceived);
        }

        void OnDestroy()
        {
            GameSignals.INPUT_GENERIC.RemoveListener(OnInputReceived);
        }

        void OnInputReceived(ISignalParameters parameters)
        {
            var type = (GestureType)parameters.GetParameter(GameParams.INPUT_TYPE);
            string textToDisplay = "";
            switch(type)
            {
                case GestureType.SWIPE:
                    var dir = (TKSwipeDirection)parameters.GetParameter(GameParams.INPUT_SWIPE_DIR);
                    textToDisplay = string.Format("Swipe detected! direction is {0}", dir.ToString());
                    break;
                default:
                    textToDisplay = string.Format("{0} Gesture detected!", type.ToString());
                    break;
            }
            DispatchText(textToDisplay);
        }

        void DispatchText(string text)
        {
            var sig = GameSignals.DEBUG_LOG;
            sig.AddParameter(GameParams.DEBUG_TEXT, text);
            sig.Dispatch();
            sig.ClearParameters();
        }
    }
}