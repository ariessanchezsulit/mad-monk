using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Common.Signal;

namespace Game
{
    public class IPDisplay : MonoBehaviour
    {
        public Text hostDisplay;

        void Awake() { GameSignals.DISPLAY_HOST_IP.AddListener(OnDisplayHostIp); }
        void OnDestroy() { GameSignals.DISPLAY_HOST_IP.RemoveListener(OnDisplayHostIp); }

        void OnDisplayHostIp(ISignalParameters parameters)
        {
            var ip = (string)parameters.GetParameter(GameParams.NETWORK_HOST_IP);
            hostDisplay.text = "Host IP: " + ip;
        }
    }
}