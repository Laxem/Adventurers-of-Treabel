using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class LivingHandler : NetworkBehaviour{

    protected Rigidbody rigidb;
    protected Vector3 moveDir;
    protected Vector3 lookDir;
    protected float moveSpeed = 5f;
    protected float rotationSpeed;

    [SyncVar]
    protected int maxLifePoint = 100;
    [SyncVar]
    protected int lifePoint = 100;
    [SyncVar]
    protected float strength = 4;

    // Use this for initialization
    protected void Init () {
        rigidb = GetComponent<Rigidbody>();
        moveDir = Vector3.zero;
    }

    // Update is called once per frame
    protected virtual void Update () {
        Control();

        Move();
	}

    protected virtual void Control()
    {
        
    }

    protected virtual void Move()
    {
        moveDir.Set(moveDir.x, 0f, moveDir.z);
        moveDir = moveDir.normalized * moveSpeed * Time.deltaTime;
        rigidb.MovePosition(transform.position + moveDir);
    }

    protected virtual void Turn()
    {
        lookDir.Set(lookDir.x, 0f, lookDir.z);
        lookDir = lookDir.normalized * moveSpeed * Time.deltaTime;
        rigidb.MovePosition(transform.position + lookDir);
    }
}
