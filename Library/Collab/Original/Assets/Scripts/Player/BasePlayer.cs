using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public InventoryObject generatedAttributes;
    Item attrHolder = null;
    bool generated;

    void OnEnable() {
        if (attrHolder == null)
            attrHolder = new Item(gameObject.GetComponent<PlayerClass>().classAttributes);
        
        generatedAttributes.Clear();
        generatedAttributes.AddItem(attrHolder);
        generatedAttributes.Save();
    }
}