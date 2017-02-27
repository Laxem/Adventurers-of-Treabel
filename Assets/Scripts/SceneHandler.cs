using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    CoopNetworkManager networkM;

    // Use this for initialization
    void Start () {
        SetNetworkInput();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void newGame() {
        GameInfo.levelHeight = 10;
        SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Single);
    }

    void SetNetworkInput()
    {
        Scene scene = SceneManager.GetActiveScene();

        networkM = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<CoopNetworkManager>();
        if (scene.name == "MenuScene")
        {
            networkM.ip = GameObject.Find("Ip Text").GetComponent<Text>();
            networkM.portClient = GameObject.Find("Client Port Input").GetComponent<Text>();
            networkM.portServer = GameObject.Find("Server Port Input").GetComponent<Text>();
            networkM.menuH = GameObject.Find("Menu Camera").GetComponent<MenuHandler>();
        }

        //ensure no connection remain active
        networkM.Quit();
    }

    public void StartUpHost()
    {
        networkM.StartUpHost();
    }

    public void StartUpClient()
    {
        networkM.StartUpClient();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

public class GameInfo : MonoBehaviour {

    public static float levelHeight = 0;
    public static string PlayerName;

    void Awake() {
        DontDestroyOnLoad(this);
    }

}