using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : LivingHandler {

    private HealthUIHandler healthUI;

    // Use this for initialization
    void Start () {
        Debug.Log("creation health bar");
        CreateHealthBar();
    }

    // Update is called once per frame
    protected override void Update () {
        healthUI.SetHealth(lifePoint, maxLifePoint);
    }

    private void CreateHealthBar()
    {
        Debug.Log("health bar npc");
        Object OHealthB = Resources.Load("Prefabs/Health Bar");
        GameObject healthB = Instantiate(OHealthB) as GameObject;
        healthUI = healthB.GetComponent<HealthUIHandler>();
        healthUI.SetParent(transform);
    }
}
