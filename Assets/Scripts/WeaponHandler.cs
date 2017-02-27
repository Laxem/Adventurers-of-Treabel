using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponHandler : MonoBehaviour
{

    PlayerHandler playerH;
    int playerColliderID;

    private float lengthSword = 1f;
    private float lengthHandle = 0.2f;

    public List<int> ColliderList = new List<int>();

    // Use this for initialization
    void Start()
    {
        playerColliderID = playerH.GetComponent<Collider>().GetInstanceID();
        //transform.position += Vector3.up * (lengthSword/2-lengthHandle);
    }


    void Update()
    {
        if (!playerH.isAttacking)
        {
            if (ColliderList.Count > 0)
            {
                for (int i = 0; i < ColliderList.Count; i++) Debug.Log("Colliderlist content " + ColliderList[i]);
            }
            ColliderList.Clear();
        }
        transform.localPosition = new Vector3(-0.1f * 4400, 0.06f * 4400, 0.06f * 4400);
        //Debug.Log(transform.position + " " + transform.localPosition);
    }

    public void Attack()
    {
        GetComponent<Animator>().SetTrigger("Attack1 0");
    }

    public void SetHolder(PlayerHandler player)
    {
        playerH = player;
    }

    public void SetParent(Transform parent)
    {
        Vector3 tmpPosition = transform.localPosition;
        Debug.Log("lcoal pos "+tmpPosition);
        transform.SetParent(parent);

        transform.localPosition = new Vector3(-0.1f*44, 0.06f*44, 0.06f*44);
        Debug.Log(transform.position+" "+transform.localPosition);
        Debug.Log(transform.parent.position + " " + transform.parent.name);
    }

    public int GetHolderId()
    {
        return transform.parent.GetInstanceID();
    }

    public PlayerHandler GetHolderHandler()
    {
        return playerH;
    }

    public float DealDamage()
    {
        return playerH.Damage();
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("Collider " + collider.GetInstanceID() + " " + collider.tag);

        int colId = collider.GetInstanceID();

        if (playerH.isAttacking)
        {
            Debug.Log("COntatc in attack"); 
            if (colId != playerColliderID)
            {
                if (!ColliderList.Contains(colId)) {
                    Debug.Log("First Contact "+collider.tag);
                    if (collider.tag == "Foe")
                    {

                    }
                    if (collider.tag == "Player")
                    {
                        collider.gameObject.GetComponent<PlayerHandler>().TakeDamage(playerH.Damage());
                    }
                    ColliderList.Add(colId);
                }
            }
            
        }

        
    }
}