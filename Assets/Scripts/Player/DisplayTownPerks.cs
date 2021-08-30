using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class DisplayTownPerks : MonoBehaviour
{
    public GameObject dexField, intField, strField, staField;
    private GameObject[] fields;
    private string[] attributeNames;

    public void Start()
    {
        fields = new GameObject[] { dexField, intField, strField, staField };
        attributeNames = new string[] { "Dexterity", "Intellect", "Strength", "Stamina" };

        for (int i = 0; i < fields.Length; i++)
        {
            fields[i].GetComponentInChildren<TextMeshProUGUI>().text = attributeNames[i] + ": +" + PlayerPrefs.GetInt(attributeNames[i]);
        }
    }
}
