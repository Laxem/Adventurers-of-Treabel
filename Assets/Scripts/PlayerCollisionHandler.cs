using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerCollisionHandler : MonoBehaviour
{

    PlayerHandler playerH;

    // Use this for initialization
    void Start () {
        playerH = gameObject.GetComponent<PlayerHandler>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionStay(Collision collision)
    {
        
        if (collision.collider.tag == "Ground")
        {
            playerH.SetNumJump(0);
        }
    }

}
