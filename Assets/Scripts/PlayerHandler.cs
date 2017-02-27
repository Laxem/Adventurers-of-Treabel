using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHandler : LivingHandler {

    private GameHandler gameH;
    private ControlHandler controlH;
    private Camera cam;
    private AudioListener audioL;
    private float distCamera;
    private Collider collid;
    private Animator animator;
    private NetworkAnimator netAnimator;

    private WeaponHandler weaponH;

    private RaycastHit rayGround;

    private PlayerUIHandler playerUI;
    private HealthUIHandler healthUI;

    private float minXangle = -80f;
    private float maxXangle = 80f;

    private float jumpForce = 8f;
    private int numJump = 0;
    private int numJumpLimit = 1;

    private float playerHeight = 1.6f;
    private float center2footOffset;

    void Start()
    {
        InitPlayer("zhdfks");
    }

    public void InitPlayer(string newName)
    {
        base.Init();

        name = newName;
        moveSpeed = 8f;
        rotationSpeed = 5f;

        gameH = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();

        controlH = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControlHandler>();

        playerUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUIHandler>();
        InitPlayerUI();

        collid = GetComponent<Collider>();

        Camera test = GetComponentInChildren<Camera>();

        cam = GetComponentInChildren<Camera>();
        cam.transform.SetParent(this.transform);
        cam.transform.localPosition += Vector3.up * 1.5f * transform.localScale.y;
        distCamera = 1f;

        cam.enabled = false;        

        audioL = GetComponentInChildren<AudioListener>();
        audioL.enabled = false;

        animator = GetComponentInChildren<Animator>();
        netAnimator = GetComponentInChildren<NetworkAnimator>();

        InitPlayer_Height();
        CreateWeapon("Sword");

        if (isLocalPlayer || !NetworkManager.singleton.isNetworkActive)
        {
            gameH.MapSetPlayer(this);
            cam.enabled = true;
            audioL.enabled = true;
        }
        else
        {
            CreateHealthBar();
        }

    }
    

    protected override void Update () {
        
        if (!isLocalPlayer && NetworkManager.singleton.isNetworkActive)
        {
            healthUI.SetHealth(lifePoint, maxLifePoint);
            return;
        }

        base.Update();

        RaycastToGround();

        Turn();
        TurnCamera();
        Jump();
        Attack();

        MajUI();
    }

    protected void InitPlayerUI()
    {
        playerUI.SetName(name);
        playerUI.SetRole("role");
        playerUI.SetHealth(lifePoint, maxLifePoint);
    }

    protected void MajUI()
    {
        playerUI.SetHealth(lifePoint, maxLifePoint);
    }

    protected void InitPlayer_Height()
    {
        CapsuleCollider coll = GetComponent<CapsuleCollider>();
        playerHeight = coll.height;
        center2footOffset = coll.center.y - playerHeight/2;
        Debug.Log("Init Height "+playerHeight+" "+center2footOffset);
    }

    protected void CreateWeapon(string weaponName)
    {
        //GameObject hand = GameObject.FindInChild("rig|DEF-f_middle.01.R.02");
        Object Oweapon = Resources.Load("Prefabs/"+weaponName);
        GameObject weapon = Instantiate(Oweapon) as GameObject;
        weaponH = weapon.GetComponent<WeaponHandler>();
        Debug.Log("weap pos "+weapon.transform.position+" "+weapon.transform.localPosition);

        //Transform hand = transform.Find("rig|DEF-f_middle.01.R.02");
        //Transform hand = transform.Find("rig|DEF-hand.R");
        //Transform hand = transform.Find("avatar2/rig/rig|root/rig|DEF-hips/rig|DEF-spine/rig|DEF-chest/rig|DEF-shoulder.R/rig|DEF-upper_arm.01.R/rig|DEF-forearm.01.R/rig|DEF-hand.R/rig|DEF-f_middle.01.R.02");
        //Transform hand = transform.Find("Hand.R");
        Transform hand = DeepFind(transform, "Hand.R");
        weaponH.SetParent(hand);
        weaponH.SetHolder(this);
    }

    private Transform DeepFind(Transform parent, string name)
    {
        Transform result = parent.Find(name);
        if (result != null) return result;

        foreach(Transform child in parent)
        {
            result = DeepFind(child, name);
            if (result != null) return result;
        }

        return null;
    }

    private void CreateHealthBar()
    {
        Object OHealthB = Resources.Load("Prefabs/Health Bar");
        GameObject healthB = Instantiate(OHealthB) as GameObject;
        healthUI = healthB.GetComponent<HealthUIHandler>();
        healthUI.SetParent(transform);
    }

    protected override void Control()
    {
        moveDir = controlH.GetPlayerMovement();
        moveDir = moveDir.normalized;
    }

    protected override void Move()
    {        
        if (controlH.StopMoving() && numJump == 0)
        {
            float velocityY = rigidb.velocity.y;
            if (velocityY < 0) velocityY = 0f;
            rigidb.velocity -= new Vector3(0, velocityY, 0);
        }

        /*
        RaycastHit hit;
        if(rayGround.distance < 0.5  && numJump == 0)
        {
            rigidb.position = new Vector3(rigidb.position.x, rigidb.position.y - rayGround.distance, rigidb.position.z);
        }
        */
        Vector3 moveDir = controlH.GetPlayerMovement();
        moveDir = moveDir.normalized;

        moveDir.Set(moveDir.x, 0f, moveDir.z);
        Vector3 movementX = transform.forward * moveDir.x * moveSpeed * Time.deltaTime;
        Vector3 movementZ = transform.right * moveDir.z * moveSpeed * Time.deltaTime;
        
        rigidb.velocity = (movementX + movementZ) * 100 + new Vector3(0, rigidb.velocity.y, 0);
        
    }

    protected override void Turn()
    {
        float turnY = controlH.GetMouseHorizontalMovement() * rotationSpeed * Time.deltaTime;
        
        Quaternion turnRotation = Quaternion.Euler(0, turnY, 0);
        transform.rotation *= turnRotation;
        rigidb.MoveRotation(transform.rotation * turnRotation);
		//transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation*turnRotation, Time.deltaTime * 10f);
    }

    protected void Jump()
    {
        if (controlH.Jump())
        {
            if (((numJump < numJumpLimit) && (rayGround.distance < 1f)) || ((numJump+1) < numJumpLimit))
            {
                rigidb.velocity = new Vector3(rigidb.velocity.x, jumpForce, rigidb.velocity.z);
                numJump++;
            }            
        }
    }

    public void SetNumJump(int newNumJump)
    {
        numJump = newNumJump;
    }

    public bool isAttacking = false;
    private bool isAttackingMem = false;

    protected void Attack()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Strike")) isAttackingMem = true;
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Strike") && isAttackingMem)
        {
            isAttacking = false;
            isAttackingMem = false;
        }

        if (controlH.Attack1() && !isAttacking)
        {
            Debug.Log("Attack");
            GetComponent<Animator>().SetTrigger("Attack1 0");


            netAnimator.SetTrigger("Attack1 0");
            if (NetworkServer.active)
                animator.ResetTrigger("Attack1 0");

            isAttacking = true;
        }
    }

    public void TakeDamage(float attackStrength)
    {
        lifePoint -= (int)Mathf.Round(attackStrength);

        if (lifePoint < 0) lifePoint = 0;

        Debug.Log("Current life " + lifePoint);
    }

    public float Damage()
    {
        Debug.Log("DAMAGE ! "+strength);
        return strength;
    }

    private void TurnCamera()
    {
        float turnX = -controlH.GetMouseVerticalMovement() * rotationSpeed * Time.deltaTime;
        float turnY = controlH.GetMouseHorizontalMovement() * rotationSpeed * Time.deltaTime;
        turnY = 0;
        Quaternion rotCamera = Quaternion.Euler(cam.transform.eulerAngles.x+turnX, cam.transform.eulerAngles.y+turnY, 0);

        //method euler
        
        float angleX = rotCamera.eulerAngles.x;
        float angleY = rotCamera.eulerAngles.y;
        if (true) angleY = transform.eulerAngles.y;   // will be use afterward for other camera control

        checkAngle(ref angleX, minXangle, maxXangle);
        //Debug.Log(turnX + "     " + Input.GetAxis("VerticalCam") + "      " + angleX+"    "+angleY);
        cam.transform.rotation = Quaternion.Euler(angleX, angleY, 0);
		//transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(angleX, angleY, 0), Time.deltaTime * 10f);
        

        //method 2 quater (ne marche pas)
        /*
        float angleX = cam.transform.eulerAngles.x + turnX;
        float angleY = cam.transform.eulerAngles.y + turnY;

        checkAngle(ref angleX, minXangle, maxXangle);

        playerCamera.transform.rotation = Quaternion.AngleAxis(angleX, Vector3.right) * Quaternion.AngleAxis(angleY, Vector3.up);
        //playerCamera.transform.rotation *= Quaternion.AngleAxis(Y, Vector3.up);
        */

        }

    private void checkAngle(ref float angle, float minAngle, float maxAngle)
    {
        float averageAngle = (minAngle + maxAngle) / 2;

        float diffAngle = 180 - averageAngle;

        float movedAngle = (angle + diffAngle) % 360;
        float movedMin = (minAngle + diffAngle) % 360;
        float movedMax = (maxAngle + diffAngle) % 360;

        if ((movedAngle > movedMin) && (movedAngle < movedMax))
        {
            return;
        }
        else
        {
            if (movedAngle < 180) angle = minAngle;
            else angle = maxAngle;
        }
    }

    protected void RaycastToGround()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + center2footOffset +0.01f, transform.position.z);
        Physics.Raycast(pos, Vector3.down, out rayGround);
        //Debug.Log(rayGround.distance);
    }


    
    
}
