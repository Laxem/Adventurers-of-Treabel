using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class NetworkHandler : MonoBehaviour {

    //public string IP = "127.0.0.1";
    //public int port = 22201;
    public MenuHandler menuH;

    public Text ip;
    public Text portClient;
    public Text portServer;
    
    public int maxUser = 20;

    public void StartServer()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            int p;
            if(Int32.TryParse(portServer.text, out p))
            {
                //Network.InitializeServer(maxUser, p, !Network.HavePublicAddress());
                //CoopNetworkManager.StartUpHost;
                //CoopNetworkManager.singleton.StartUpHost();
                
            }
            else
            {
                menuH.ServerPortError();
            }
        }
    }

    public void StartClient()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //Network.Connect(ip.text, Int32.Parse(portServer.text));
            int p;
            if (Int32.TryParse(portClient.text, out p))
            {
                Network.Connect(ip.text, p);
            }
            else
            {
                menuH.ClientPortError();
            }
        }
    }

}
