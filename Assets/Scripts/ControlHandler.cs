using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControlHandler : MonoBehaviour {

    private CoopNetworkManager networkM;
    private GameHandler gameH;

    private bool jumping = false;
    private bool isMoving = false;

	// Use this for initialization
	void Start () {
	    networkM = GameObject.Find("Network Manager").GetComponent<CoopNetworkManager>();
        gameH = GameObject.Find("GameControl").GetComponent<GameHandler>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            //networkM.Quit();

            if (gameH.isModeGame())
            {
                gameH.ModeMenu();                
            }
            else if (gameH.isModeMenu())
            {
                gameH.ModeGame();
            }
            
        }
    }

    public Vector3 GetPlayerMovement()
    {
        Vector3 playerMove = new Vector3(Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));
        return playerMove;
    }

    public float GetMouseHorizontalMovement()
    {
        if (gameH.isModeGame()) return Input.GetAxis("HorizontalCam");
        return 0;
    }

    public float GetMouseVerticalMovement()
    {
        if (gameH.isModeGame()) return Input.GetAxis("VerticalCam");
        return 0;
    }

    public bool Jump()
    {
        if (gameH.isModeGame()) {
            bool isJumping = Input.GetButtonDown("Jump");
            return isJumping;
        }
        return false;
    }

    public bool Attack1()
    {
        if (gameH.isModeGame())
        {
            bool attack = Input.GetButtonDown("Fire1");
            return attack;
        }
        return false;
    }

    public bool StopMoving()
    {
        bool stopMoving = isMoving && !(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0);
        //stopMoving = stopMoving || (!isMoving && (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0));
        isMoving = Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0;
        return stopMoving;
    }
}
