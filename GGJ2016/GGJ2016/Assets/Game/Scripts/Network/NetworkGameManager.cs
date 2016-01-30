using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Game {
    public class NetworkGameManager : NetworkManager {
        public Text clientIdDisplay;

        //public override void OnStartClient(NetworkClient client)
        //{
        //    clientIdDisplay.text = string.Format("Client ID is: {0}", client.)
        //}
    }
}