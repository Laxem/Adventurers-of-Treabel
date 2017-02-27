using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class CoopNetworkManager : NetworkManager {

    public MenuHandler menuH;

    public Text ip;
    public Text portClient;
    public Text portServer;

    public int maxPlayer = 20;	

    void Update()
    {

    }

    public void StartUpHost()
    {
        if (!NetworkManager.singleton.isNetworkActive)
        {
            int p;
            if (Int32.TryParse(portServer.text, out p))
            {
                //Network.InitializeServer(maxUser, p, !Network.HavePublicAddress());
                singleton.networkPort = p;
                singleton.maxConnections = maxPlayer;
                GameInfo.levelHeight = 10;
                singleton.StartHost();
            }
            else
            {
                menuH.ServerPortError();
            }
        }
    }

    public void StartUpClient()
    {
        if (!NetworkManager.singleton.isNetworkActive)
        {
            //Network.Connect(ip.text, Int32.Parse(portServer.text));
            int p;
            if (Int32.TryParse(portClient.text, out p))
            {
                //Network.Connect(ip.text, p);
                singleton.networkAddress = ip.text;
                singleton.networkPort = p;
                GameInfo.levelHeight = 10;
                singleton.StartClient();
            }
            else
            {
                menuH.ClientPortError();
            }
        }
    }

    public void Quit()
    {
        //SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
        
        singleton.StopClient();
        singleton.StopHost();
        singleton.StopServer();
        
    }
   
}
