using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Common.Signal;

namespace Game
{
    public class GestureClientController : NetworkBehaviour
    {
        [SerializeField]
        private GestureManager _instance;
        [SerializeField]
        private int clientId;

        void Awake()
        {
            if(!_instance)
            {
                var go = GameObject.Find("GestureManager");
                if(go)
                {
                    _instance = go.GetComponent<GestureManager>();
                    _instance.UseNetwork = true;
                    
                }
            }

            clientId = GetComponent<NetworkIdentity>().GetInstanceID();
            
            GameSignals.INPUT_GENERIC.AddListener(OnInputReceived);
        }

        void OnDestroy()
        {
            GameSignals.INPUT_GENERIC.RemoveListener(OnInputReceived);
        }

        void OnInputReceived(ISignalParameters parameters)
        {
            if (isLocalPlayer)
            {
                var type = (GestureType)parameters.GetParameter(GameParams.INPUT_TYPE);
                string textToDisplay = "";
                switch (type)
                {
                    case GestureType.TAP:
                        var pos = (Vector2)parameters.GetParameter(GameParams.INPUT_TAP_POS);

                        // server already dispatches this on its side
                        if(!isServer)
                            CmdDispatchTap(pos);

                        CmdSpawnTapEffectToServer(pos);

                        textToDisplay = string.Format("{0} Gesture detected!", type.ToString());
                        break;
                    case GestureType.SWIPE:
                        //var dir = (TKSwipeDirection)parameters.GetParameter(GameParams.INPUT_SWIPE_DIR);
                        //textToDisplay = string.Format("Swipe detected! direction is {0}", dir.ToString());
                        //SpawnSwipeEffect(payload);

                        var payload = (SwipePayload)parameters.GetParameter(GameParams.INPUT_SWIPE_PAYLOAD);

                        // server already dispatches this on its side
                        if (!isServer)
                            CmdDispatchSwipe(payload);

                        CmdSpawnSwipeEffectToServer(payload);

                        textToDisplay = string.Format("Swipe detected! direction: {0}, startPos: {1}, endPos: {2}, velocity: {3}", payload.direction.ToString(), payload.startScreenPos, payload.endScreenPos, payload.velocity);
                        break;
                    default:
                        textToDisplay = string.Format("{0} Gesture detected!", type.ToString());
                        break;
                }
                //DispatchText(textToDisplay);
                CmdDispatchTextToServer(textToDisplay);
            }
        }

        void DispatchText(string text)
        {
            var sig = GameSignals.DEBUG_LOG;
            sig.AddParameter(GameParams.DEBUG_TEXT, text);
            sig.Dispatch();
            sig.ClearParameters();
        }

        void SpawnSwipeEffect(SwipePayload payload)
        {
            var start = Camera.main.ScreenToWorldPoint(payload.startScreenPos);
            var end = Camera.main.ScreenToWorldPoint(payload.endScreenPos);

            start = new Vector3(start.x, start.y, 0);
            end = new Vector3(end.x, end.y, 0);

            if (_instance)
            {
                StartCoroutine(_instance.SpawnSwipeEffect(start, end, 0.5f, payload.velocity));
            }
        }

        void SpawnTapEffect(Vector2 position)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(position);
            if(_instance)
            {
                StartCoroutine(_instance.SpawnTapEffect(worldPos));
            }
        }

        #region command wrappers
        [Command]
        void CmdDispatchTextToServer(string text)
        {
            //string clientId = GetComponent<NetworkIdentity>().GetInstanceID().ToString();
            var sig = GameSignals.DEBUG_LOG;
            sig.AddParameter(GameParams.DEBUG_TEXT, string.Format("[client#{0}]: {1}", clientId, text));
            sig.Dispatch();
            sig.ClearParameters();
        }

        [Command]
        void CmdSpawnSwipeEffectToServer(SwipePayload payload)
        {
            SpawnSwipeEffect(payload);
        }

        [Command]
        void CmdSpawnTapEffectToServer(Vector2 position)
        {
            SpawnTapEffect(position);
        }

        [Command]
        void CmdDispatchSwipe(SwipePayload payload)
        {
            //if (_instance) _instance.DispatchNetworkSwipe(payload);
            Debug.Log("Client Swipe detected");

            var signal = GameSignals.INPUT_NETWORK;
            signal.AddParameter(GameParams.INPUT_TYPE, GestureType.SWIPE);
            signal.Dispatch();
            signal.ClearParameters();
        }

        [Command]
        void CmdDispatchTap(Vector2 position)
        {
            //if (_instance) _instance.DispatchNetworkTap(position);
            Debug.Log("Client Tap detected");

            var signal = GameSignals.INPUT_NETWORK;
            signal.AddParameter(GameParams.INPUT_TYPE, GestureType.TAP);
            signal.Dispatch();
            signal.ClearParameters();
        }
        #endregion
    }
}