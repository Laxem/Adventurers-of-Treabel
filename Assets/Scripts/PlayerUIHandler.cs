using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIHandler : MonoBehaviour {

    private GameHandler gameH;

    public Text playerName;
    public Image playerRole;
    public Slider playerHealth;

    public Canvas menu;

    void Start()
    {
        gameH = GameObject.Find("GameControl").GetComponent<GameHandler>();
        HideMenu();
    }

    public void SetName(string name)
    {
        playerName.text = name;
    }

    public void SetRole(string role)
    {
        
    }

    public void SetHealth(int health, int maxHealth)
    {
        playerHealth.value = health / maxHealth;
        if(playerHealth.value > 0.4f)
        {
            playerHealth.GetComponentInChildren<Image>().color = new Color(21f/255f, 234f / 255f, 31f / 255f);
        }
        else if (playerHealth.value > 0.1f)
        {
            playerHealth.GetComponentInChildren<Image>().color = new Color(234f / 255f, 211f / 255f, 21f / 255f);
        }
        else
        {
            playerHealth.GetComponentInChildren<Image>().color = new Color(234f / 255f, 21f / 255f, 21f / 255f);
        }
    }

    public void ShowMenu() {
        menu.enabled = true;
    }

    public void HideMenu()
    {
        menu.enabled = false;
    }

    public void BackToMenu()
    {
        gameH.ExitGame();
    }
}
