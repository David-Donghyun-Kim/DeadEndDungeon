using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class DisplayPlayerAttributes : MonoBehaviour
{
    public DisplayListener parent;

    [Serializable]
    public class AttributeObjectPair {
        public BuffType attribute;
        public GameObject gameObject;
    }

    public List<AttributeObjectPair> attributeToGUIList;
   
    void Start()
    {
        foreach (Attribute att in parent.atts.attributes) {
            att.value.RegisterModEvent(OnAttributeChanged);
        }
        OnAttributeChanged();
    }

    public void OnAttributeChanged()
    {
        foreach (Attribute att in parent.atts.attributes)
        {
            foreach (AttributeObjectPair pair in attributeToGUIList) {
                if (pair.attribute == att.type) {
                    string displayString = att.type.ToString() + ": " + (att.value.BaseValue + att.value.ModifiedValue).ToString();
                    if (pair.gameObject != null) {
                        pair.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = displayString;
                    }
                }
            }
        }
    }
}
