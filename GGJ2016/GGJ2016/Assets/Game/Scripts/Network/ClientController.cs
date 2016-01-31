using UnityEngine;
using System.Collections;
using Common.Signal;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Game
{
    public class ClientController : MonoBehaviour
    {
        public GameObject hostInput;
        public GameObject stopButton;
        public Text ipInput;
        public Text networkStatusDisplay;
        public NetworkGameManager manager;

        void Awake()
        {
            hostInput.SetActive(true);
            stopButton.SetActive(false);

            GameSignals.NETWORK_STATUS_SIGNAL.AddListener(OnNetworkStatusReceived);
        }

        void OnDestroy()
        {
            GameSignals.NETWORK_STATUS_SIGNAL.RemoveListener(OnNetworkStatusReceived);
        }

        public void TryConnectToHost()
        {
            var ip = ipInput.text;
            manager.JoinLocalGame(ip);
        }

        public void StopConnection()
        {
            manager.StopHost();
            hostInput.SetActive(true);
            stopButton.SetActive(false);

            GameSignals.END_GAME.Dispatch();
        }

        void OnNetworkStatusReceived(ISignalParameters parameters)
        {
            var status = (string)parameters.GetParameter(GameParams.NETWORK_STATUS);

            networkStatusDisplay.text = string.Format("Connection status: {0}", status);

            if (status.Contains("success")) OnJoinSuccess();
            else OnJoinFailed();
        }

        void OnJoinSuccess()
        {
            hostInput.SetActive(false);
            stopButton.SetActive(true);

            GameSignals.START_GAME.Dispatch();
        }

        void OnJoinFailed()
        {
            hostInput.SetActive(true);
            stopButton.SetActive(false);
        }
    }
}