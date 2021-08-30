using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateAbilityDescription : MonoBehaviour
{
    public InventoryObject basePlayerAttributes;


    //ik this script is written in a dumb way... i just want shit to work at this point :^)
    void Awake()
    {
        int classSelected = PlayerPrefs.GetInt("CharacterSelected");
        if (classSelected == 0) {
             transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: Consume enemies" 
            + "\n" + "Passive: High critical strike chance";
        } else if (classSelected == 1 || classSelected == 9) {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: Invisibility (avoidance) for 5 seconds"
            + "\n" + "Passive: [future patch]";
        }else if (classSelected == 2 || classSelected == 6) {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: Take 50% damage reduction for 10 seconds [in testing]"
            + "\n" + "Passive: [future patch]";
        } else if (classSelected == 3 || classSelected == 8) {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: [future patch]";
        } else if (classSelected == 4 || classSelected == 7) {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: [future patch]";
        } else if (classSelected == 5 || classSelected == 11) {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: [future patch]";
        } else if (classSelected == 10) {
             transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: Receive gold from kills";
        } 
    }

    public void Toggle()
    {
        int classSelected = PlayerPrefs.GetInt("CharacterSelected");
        string className = basePlayerAttributes.getSlots[1].item.name;
        if (className == "Hunter") {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: Consume enemies" 
            + "\n" + "Passive: High critical strike chance";
        } else if (className == "Rogue") {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: Invisibility (avoidance)"
            + "\n" + "Passive: [future patch]";
        }else if (className == "Warrior") {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: Take 50% damage reduction for 10 seconds [in testing]"
            + "\n" + "Passive: [future patch]";
        } else if (className == "Peasant") {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: [future patch]";
        } else if (className == "Distiller") {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: [future patch]";
        } else if (className == "Mage") {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: [future patch]";
        } else if (className == "Mercenary") {
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Active: [future patch]"
            + "\n" + "Passive: Receive gold from kills";
        } 
    }
}