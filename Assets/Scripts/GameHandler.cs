using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameHandler : MonoBehaviour {

    private GameObject map;
    private GameObject player;
    private GameObject playerView;

    public PlayerUIHandler playerUiH;

    private MapHandler mapH;
        
    private Vector3 initPosplayer;
    public string worldName;
    enum Mode { game, menu };
    private int mode = (int)Mode.game;
    

    // Use this for initialization
    void Start () {
        Object Omap = Resources.Load("Prefabs/Map");
        map = Instantiate(Omap) as GameObject;

        if (!NetworkManager.singleton.isNetworkActive)
        {
            Object Oplayer = Resources.Load("Prefabs/Player");
            player = Instantiate(Oplayer) as GameObject;
        }

        mapH = map.GetComponent<MapHandler>();

        worldName = "world1";
        initPosplayer = new Vector3(0, 10, 0);

        InitWorld();

        HandleCursor();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void InitWorld()
    {
        //player.GetComponent<PlayerHandler>().InitPlayer("Player1");
        //player.transform.position = initPosplayer;
        mapH.InitMap(this);      
    }

    public void MapSetPlayer(PlayerHandler player)
    {
        map.GetComponent<MapHandler>().SetPlayer(player);
    }
    
    public void ExitGame()
    {
        SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
    }

    public void ModeGame()
    {
        mode = (int)Mode.game;
        HandleCursor();
        playerUiH.HideMenu();
    }

    public void ModeMenu()
    {
        mode = (int)Mode.menu;
        HandleCursor();
        playerUiH.ShowMenu();
    }

    public bool isModeGame()
    {
        return mode == (int)Mode.game;
    }
    public bool isModeMenu()
    {
        return mode == (int)Mode.menu;
    }

    private void HandleCursor()
    {
        Cursor.visible = mode==(int)Mode.menu;
        if (mode == (int)Mode.game) Cursor.lockState = CursorLockMode.Locked;
        else if (Screen.fullScreen) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.None;
    }

}
