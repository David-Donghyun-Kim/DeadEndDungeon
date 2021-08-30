using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class DisplayPlayerClass : MonoBehaviour
{
    public Sprite[] portraits;
    public string[] classNames;

    void Start()
    {
        transform.GetChild(2).GetComponent<Image>().sprite = portraits[PlayerPrefs.GetInt("CharacterSelected")];
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = classNames[PlayerPrefs.GetInt("CharacterSelected")];
    }
}
