using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
public class ItemObject : ScriptableObject
{

    public Sprite uiDisplay;
    public bool stackable;
    //public int amount;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item();

    public ItemObject() {
        
    }

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}