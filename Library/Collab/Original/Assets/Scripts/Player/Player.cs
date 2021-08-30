using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject baseAttributeInventory;
    public PlayerCurrentWeapon currentWeapon;
    public Attribute[] attributes;
    public Camera cameraCast;
    public StatusCondition condition;
    private float rayDistance;
    private float distance = 13;
    public int health = 10;
    /**/
    // private void UpdateStats()
    // {
    //     UpdateStats();
    // }
	
	public void Start()
	{	
		for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < inventory.getSlots.Length; i++)
        {
            inventory.getSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            inventory.getSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        condition = StatusCondition.None;
        inventory.SetStartingWeapon();
        Debug.Log("weapon is " + inventory.equippedWeapon);
        currentWeapon.weaponType = inventory.equippedWeapon;
        currentWeapon.attack_type = inventory.equippedWeaponAttackType;

        foreach (InventorySlot slot in baseAttributeInventory.getSlots) {
            Debug.Log("player class is " + slot.item.name);
            for (int i = 0; i < slot.item.buffs.Length; i++)
                for (int j = 0; j < attributes.Length; j++)
                    if (attributes[j].type == slot.item.buffs[i].attribute)
                        attributes[j].value.AddModifier(slot.item.buffs[i]);
        }
	}

    public void OnBeforeSlotUpdate (InventorySlot _slot)
    {
        if (_slot.GetItemObject == null)
            return;
        
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
            case InterfaceType.Weapon:
                print(string.Concat("Removed ", _slot.GetItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", 
                string.Join(",", _slot.allowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

     public void OnAfterSlotUpdate (InventorySlot _slot)
    {
        if (_slot.GetItemObject == null)
            return;

        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
            case InterfaceType.Weapon:
                print(string.Concat("Placed ", _slot.GetItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", 
                string.Join(",", _slot.allowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            // ex. players dexterity + items dexterity
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            inventory.Save();
            baseAttributeInventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inventory.Load();
            baseAttributeInventory.Load();
        }
		
		//if (attributes[0].value.ModifiedValue == 0){ UpdateStats(); }
		
		for (int j = 0; j < attributes.Length; j++) 
        {
            PlayerPrefs.SetFloat(attributes[j].type.ToString(), attributes[j].value.ModifiedValue);
        }
    }

    public int GetHealth() {
        // use stamina however you want to calculate health
        int stam = GetAttributeAsInt(BuffType.Stamina);

        // if you want to use some sort of base value for health,
        // this might be useful (set through the class attribute item objects)
        // otherwise, just comment it out
        int baseHealth = GetAttributeAsInt(BuffType.Health);

        return health; 
    }
    
    public void PickUp () {
        int layerMask = 1 << LayerMask.NameToLayer("Loot");
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = cameraCast.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, distance, layerMask))
        {
            if (raycastHit.collider.gameObject.tag == "Item") {
                ObtainItem(raycastHit);
            } else if (raycastHit.collider.gameObject.tag == "Weapon") {
                ObtainWeapon(raycastHit);
            } else if (raycastHit.collider.gameObject.tag == "Enemy") {
                gameObject.BroadcastMessage("OnInteract", raycastHit.collider.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    
     public void ObtainItem(RaycastHit raycastHit)
    {
        var groundItem = raycastHit.collider.gameObject.GetComponent<GroundItem>();
        Item _item = new Item(groundItem.item);
        if (inventory.AddItem(_item))
            Destroy(raycastHit.collider.gameObject);
    }
    
    public void ObtainWeapon(RaycastHit raycastHit)
    {
        var groundItem = raycastHit.collider.GetComponent<GroundItem>();

        if (currentWeapon.weaponType != WeaponType.None) // if  a weapon is equipped
            inventory.SwitchWeapon(raycastHit.collider);

        currentWeapon.weaponType = WeaponType.None;
        currentWeapon.attack_type = WeaponAttackType.None;
        
        Item _item = new Item(groundItem.item);
        if (inventory.AddItem(_item))
        {
            currentWeapon.weaponType = inventory.equippedWeapon;
            currentWeapon.attack_type = inventory.equippedWeaponAttackType;
            Debug.Log(currentWeapon.attack_type);
            Destroy(raycastHit.collider.gameObject);
        }
    }


    public void AttributeModified (Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated. Value is now: ", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        baseAttributeInventory.Clear();
    }

    public int GetAttributeAsInt(BuffType att)
    {       
        for (int i = 0; i < attributes.Length; i++)
            if (attributes[i].type == att)
                return attributes[i].value.ModifiedValue;

        return 0;
    }
}
