using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour {

    public Slider healthB;

    private Camera displayCamera;

    void Start()
    {
        displayCamera = Camera.current;
        
    }

    void Update()
    {
        /*
        Vector3 v = displayCamera.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(displayCamera.transform.position - v);
        transform.Rotate(0, 180, 0);
        */
    }

    public void SetHealth(int health, int maxHealth)
    {
        healthB.value = health / maxHealth;
        if (healthB.value > 0.4f)
        {
            healthB.GetComponentInChildren<Image>().color = new Color(21f / 255f, 234f / 255f, 31f / 255f);
        }
        else if (healthB.value > 0.1f)
        {
            healthB.GetComponentInChildren<Image>().color = new Color(234f / 255f, 211f / 255f, 21f / 255f);
        }
        else
        {
            healthB.GetComponentInChildren<Image>().color = new Color(234f / 255f, 21f / 255f, 21f / 255f);
        }
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
    }


}
