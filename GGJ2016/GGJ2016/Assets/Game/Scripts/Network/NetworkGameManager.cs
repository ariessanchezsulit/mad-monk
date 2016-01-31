using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using Common.Signal;
using System.Net;

namespace Game {
    public enum ConnectionType
    {
        SERVER,
        CLIENT
    }

    public class NetworkGameManager : NetworkManager
    {
        public ConnectionType connType;
        public string hostIp;
                
        void Awake()
        {
            GameSignals.INPUT_HOST_IP.AddListener(OnInputHostIP);
            GameSignals.START_GAME.AddListener(OnGameStarted);
            GameSignals.END_GAME.AddListener(OnGameEnded);
        }

        void OnDestroy()
        {
            GameSignals.INPUT_HOST_IP.RemoveListener(OnInputHostIP);
            GameSignals.START_GAME.RemoveListener(OnGameStarted);
            GameSignals.END_GAME.RemoveListener(OnGameEnded);
        }

        void Start()
        {
            //StartCoroutine(DelayStartServer(1));
        }

        IEnumerator DelayStartServer(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (connType == ConnectionType.SERVER) CreateLocalGame();
        }

        public void CreateLocalGame()
        {
            this.StartHost();
            Debug.Log("host started");
            hostIp = GetLocalIPAddress();
            Debug.Log("host ip is " + hostIp);

            var signal = GameSignals.DISPLAY_HOST_IP;
            signal.AddParameter(GameParams.NETWORK_HOST_IP, hostIp);
            signal.Dispatch();
            signal.ClearParameters();

            var networkSignal = GameSignals.NETWORK_STATUS_SIGNAL;
            if(NetworkServer.active)
            {
                networkSignal.AddParameter(GameParams.NETWORK_STATUS, "Server Active");
            }
            else
            {
                networkSignal.AddParameter(GameParams.NETWORK_STATUS, "Server Failed");
            }
            networkSignal.Dispatch();
            networkSignal.ClearParameters();
        }

        public void JoinLocalGame(string hostIp)
        {
            this.StartClient();
            networkAddress = hostIp;

            var signal = GameSignals.NETWORK_STATUS_SIGNAL;
            if(NetworkClient.active)
            {
                signal.AddParameter(GameParams.NETWORK_STATUS, "Join success");
            }
            else
            {
                signal.AddParameter(GameParams.NETWORK_STATUS, "Join failed");
            }
            signal.Dispatch();
            signal.ClearParameters();
        }
        
        public override void OnStartHost()
        {
            Debug.Log("Host started");
        }

        public override void OnStopHost()
        {
            Debug.Log("Host stopped");
        }

        public string GetLocalIPAddress()
        {
            IPHostEntry host;
            string localIp = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList)
            {
                if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            return localIp;
        }

        void OnGameStarted(ISignalParameters parameters)
        {
            if (connType == ConnectionType.SERVER) CreateLocalGame();
        }

        void OnGameEnded(ISignalParameters parameters)
        {
            if (connType == ConnectionType.SERVER)
            {
                this.StopHost();
                Debug.Log("host stopped");

                var networkSignal = GameSignals.NETWORK_STATUS_SIGNAL;
                networkSignal.AddParameter(GameParams.NETWORK_STATUS, "Host Stopped");
                networkSignal.Dispatch();
                networkSignal.ClearParameters();
            }
        }

        void OnInputHostIP(ISignalParameters parameters)
        {
            if (connType != ConnectionType.CLIENT) return;

            hostIp = (string)parameters.GetParameter(GameParams.NETWORK_HOST_IP);
            JoinLocalGame(hostIp);
        }
    }
}