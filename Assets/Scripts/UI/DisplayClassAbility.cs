using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayClassAbility : MonoBehaviour
{
    private GameObject cooldownPanel, imagePanel;
    private int timer;
    private Color imageColor;
    // Start is called before the first frame update
    void Awake()
    {
        this.cooldownPanel = GameObject.Find("/Player Unit Frame/Panel/AbilityCooldown");
        this.imagePanel = cooldownPanel.transform.GetChild(0).gameObject;
        imageColor = imagePanel.GetComponent<Image>().color;
    }

    IEnumerator StartCooldownTimer()
    {
        Debug.Log("Entered coroutine: Timer is " + this.timer + " seconds");
        imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, 0.4f);
        imagePanel.GetComponent<Image>().color = imageColor;
        while (timer > 0)
        {
            Debug.Log(timer + " seconds left");
            cooldownPanel.GetComponent<TextMeshProUGUI>().text = timer.ToString();
            yield return new WaitForSeconds(1.0f);
            timer--;
        }
        imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, 1.0f);
        imagePanel.GetComponent<Image>().color = imageColor;
        cooldownPanel.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void StartAbilityCooldown(int time)
    {
        this.timer = time;
        Debug.Log("Starting Coroutine: StartCooldownTimer. Timer is set to " + this.timer);
        StartCoroutine("StartCooldownTimer");

    }
}
