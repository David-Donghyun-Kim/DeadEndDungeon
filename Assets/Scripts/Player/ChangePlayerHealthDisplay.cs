using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerHealthDisplay : MonoBehaviour
{
    private int playerHealth, maxPlayerHealth;
    private GameObject panel;
    private GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        maxPlayerHealth = GameObject.Find("Player").GetComponent<Player>().GetMaxHealth();
        playerHealth = maxPlayerHealth;
        panel = this.transform.GetChild(0).transform.GetChild(0).gameObject;
        text = this.transform.GetChild(2).gameObject;

        text.GetComponent<TextMeshProUGUI>().text = playerHealth + "/" + maxPlayerHealth;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     text.GetComponent<TextMeshPro>().text = playerHealth + "/" + maxPlayerHealth;
    // }

    public void SetCurrentHealth(int i) { 
        playerHealth = i;
        AdjustHealthDisplay(playerHealth, maxPlayerHealth);
    }
    public void SetMaxCurrentHealth(int i) { 
        maxPlayerHealth = i;
        AdjustHealthDisplay(playerHealth, maxPlayerHealth);
    }

    private void AdjustHealthDisplay(int current, int max)
    {
        panel.GetComponent<Image>().fillAmount = (float)playerHealth / (float)maxPlayerHealth;
        text.GetComponent<TextMeshProUGUI>().text = current + "/" + max;
    }
}
