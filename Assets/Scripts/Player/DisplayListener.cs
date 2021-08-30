using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayListener : MonoBehaviour
{
    public Attributes atts;
    public InventoryObject inventory;
    public InventoryObject baseAttributeInventory;

    public void Start()
	{	
        for (int i = 0; i < atts.attributes.Length; i++) {
            atts.attributes[i].SetParent(this);
        }


        for (int i = 0; i < inventory.getSlots.Length; i++)
        {
            inventory.getSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            inventory.getSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        for (int i = 0; i < baseAttributeInventory.getSlots.Length; i++)
        {
            baseAttributeInventory.getSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            baseAttributeInventory.getSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        foreach (InventorySlot slot in baseAttributeInventory.getSlots) {
            Debug.Log("player class is " + slot.item.name);
            for (int i = 0; i < slot.item.buffs.Length; i++)
                for (int j = 0; j < atts.attributes.Length; j++)
                    if (atts.attributes[j].type == slot.item.buffs[i].attribute)
                        atts.attributes[j].value.AddModifier(slot.item.buffs[i]);
        }
    }

    // public void OnDungeonEntered() {
    //     for (int i = 0; i < baseAttributeInventory.getSlots.Length; i++)
    //     {
    //         baseAttributeInventory.getSlots[i].OnBeforeUpdate -= OnBeforeSlotUpdate;
    //         baseAttributeInventory.getSlots[i].OnAfterUpdate -= OnAfterSlotUpdate;
    //     }
    // }
    
    public void OnBeforeSlotUpdate (InventorySlot _slot)
    {
        if (_slot.GetItemObject == null)
            return;
        
        print(string.Concat("Removed ", _slot.GetItemObject, " on ", _slot.parent.inventory, ", Allowed Items: ", string.Join(",", _slot.allowedItems)));

        for (int i = 0; i < _slot.item.buffs.Length; i++)
        {
            for (int j = 0; j < atts.attributes.Length; j++)
            {
                if (atts.attributes[j].type == _slot.item.buffs[i].attribute)
                    atts.attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
            }
        }
    }

    public void OnAfterSlotUpdate (InventorySlot _slot)
    {
       if (_slot.GetItemObject == null)
            return;

        print(string.Concat("Placed ", _slot.GetItemObject, " on ", _slot.parent.inventory, ", Allowed Items: ", 
        string.Join(",", _slot.allowedItems)));

        for (int i = 0; i < _slot.item.buffs.Length; i++)
        {
            for (int j = 0; j < atts.attributes.Length; j++)
            {
                if (atts.attributes[j].type == _slot.item.buffs[i].attribute)
                    // ex. players dexterity + items dexterity
                    atts.attributes[j].value.AddModifier(_slot.item.buffs[i]);
            }
        }
    }

    public void AttributeModified (Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated. Value is now: ", attribute.value.ModifiedValue));
    }

    void OnDisable() {
        for (int i = 0; i < baseAttributeInventory.getSlots.Length; i++)
        {
            baseAttributeInventory.getSlots[i].OnBeforeUpdate -= OnBeforeSlotUpdate;
            baseAttributeInventory.getSlots[i].OnAfterUpdate -= OnAfterSlotUpdate;
        }

        for (int i = 0; i < inventory.getSlots.Length; i++)
        {
            inventory.getSlots[i].OnBeforeUpdate -= OnBeforeSlotUpdate;
            inventory.getSlots[i].OnAfterUpdate -= OnAfterSlotUpdate;
        }
    }
}
