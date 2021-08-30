using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class DisplayBaseStats : MonoBehaviour
{

    public GameObject charList;
    InventoryObject attributesHolder;

    [Serializable]
    public class AttributeObjectPair {
        public BuffType attribute;
        public GameObject gameObject;
    }

    public List<AttributeObjectPair> attributeToGUIList;

    public void Start() {
        CharacterToggled(true);
    }

    public void CharacterToggled(Boolean isStartingCharacter) {
        // wrong class stats show up on first character if this bool isnt used
        if (!isStartingCharacter) {
            attributesHolder = charList.transform.GetChild(PlayerPrefs.GetInt("CharacterSelected")).GetComponent<BasePlayer>().generatedAttributes;
        } else {
            attributesHolder = charList.GetComponentInChildren<BasePlayer>().generatedAttributes;
        }

        foreach (ItemBuff buff in attributesHolder.container.slots[0].item.buffs) {
              foreach (AttributeObjectPair pair in attributeToGUIList) {
                if (pair.attribute == buff.attribute) {
                    string displayString = buff.attribute.ToString() + ": " + buff.value.ToString();
                    pair.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = displayString;
					PlayerPrefs.SetFloat(buff.attribute.ToString(), buff.value);
                }
            }
        }
    }
}
