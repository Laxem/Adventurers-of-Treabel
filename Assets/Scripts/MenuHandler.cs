using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuHandler : MonoBehaviour {

    public Canvas MainMenuCanvas;
    public Canvas ClientMenuCanvas;
    public Canvas ServerMenuCanvas;

    public Text portClient;
    public Text portServer;

    void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        DisableAllMenu();
        MainMenuCanvas.enabled = true;
    }

    public void ClientMenu () {
        DisableAllMenu();
        ClientMenuCanvas.enabled = true;
	}

    public void ServerMenu()
    {
        DisableAllMenu();
        ServerMenuCanvas.enabled = true;
    }

    public void ClientPortError()
    {
        portClient.enabled = true;
    }

    public void ServerPortError()
    {
        portServer.enabled = true;
    }

    void DisableAllMenu()
    {
        MainMenuCanvas.enabled = false;
        ClientMenuCanvas.enabled = false;
        ServerMenuCanvas.enabled = false;
        portClient.enabled = false;
        portServer.enabled = false;
    }

}
