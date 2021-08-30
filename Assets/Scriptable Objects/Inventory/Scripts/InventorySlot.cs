using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

public delegate void SlotChangeEvent(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotChangeEvent OnAfterUpdate;
    [System.NonSerialized]
    public SlotChangeEvent OnBeforeUpdate;
    public Item item = new Item();
    public int amount;

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void UpdateSlot(Item _item, int _amount)
    {
        //if (OnBeforeUpdate != null && parent != null)
        if(OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);

        item = _item;
        amount = _amount;
        
        //if (OnAfterUpdate != null && parent != null)
        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }
    
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (allowedItems.Length <= 0 || _itemObject == null || _itemObject.data.ID < 0)
            return true;
        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (_itemObject.type == allowedItems[i])
                return true;
        }
        return false;
    }

    public ItemObject GetItemObject
    {
        get
        {
            if (item.ID >= 0)
            {
                if (parent == null) 
                    Debug.Log("parent is null");
                return parent.inventory.database.itemObjects[item.ID];
            }
            return null;
        }
    }
}