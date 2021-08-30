using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory container;
    [SerializeField]
    public ItemObject startingWeapon;
    public GameObject[] weapons;
    public InventorySlot[] getSlots { get { return container.slots; } }
    public WeaponType equippedWeapon;
    public WeaponAttackType equippedWeaponAttackType;

    public void SetStartingWeapon()
    {
        AddItem(startingWeapon.data);
    }

    public bool AddItem(Item _item)
    {
        if (EmptySlotCount <= 0)
            return false;

        InventorySlot slot = FindItemOnInventory(_item.ID);
        Debug.Log(_item.name + ", " + _item.ID);
        if (!database.itemObjects[_item.ID].stackable || slot == null)
        {
            // if item is a weapon
            if (_item.weaponType != WeaponType.None)
            {
                Debug.Log(_item.attackType);
                // if no item is currently equipped
                if (equippedWeapon == WeaponType.None)
                {
                    equippedWeapon = _item.weaponType;
                    equippedWeaponAttackType = _item.attackType; ;
                    SetEmptySlot(_item, _item.amount);
                }
                else
                {
                    // if weapon is currently equipped, get rid of it and add new item
                    InventorySlot weaponSlot = FindWeaponOnInventory();
                    weaponSlot.RemoveItem();
                    SetEmptySlot(_item, _item.amount);
                    equippedWeapon = _item.weaponType;
                    equippedWeaponAttackType = _item.attackType; ;
                }
            }
            else
            {
                SetEmptySlot(_item, _item.amount);
            }

            return true;
        }
        slot.AddAmount(_item.amount);

        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < getSlots.Length; i++)
            {
                if (getSlots[i].item.ID <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    // Returns the first item of the specified id
    public InventorySlot FindItemOnInventory(int id)
    {
        for (int i = 0; i < getSlots.Length; i++)
        {
            if (getSlots[i].item.ID == id)
            {
                return getSlots[i];
            }
        }
        return null;
    }

    // Returns an array of all items of the specified id
    public List<InventorySlot> FindAllItemsonInventory(int id)
    {
        List<InventorySlot> items = new List<InventorySlot> { };
        for (int i = 0; i < getSlots.Length; i++)
        {
            if (getSlots[i].item.ID == id)
            {
                items.Add(getSlots[i]);
            }
        }
        return items;
    }

    public InventorySlot FindWeaponOnInventory()
    {
        for (int i = 0; i < getSlots.Length; i++)
        {
            if (getSlots[i].item.weaponType != WeaponType.None)
            {
                return getSlots[i];
            }
        }
        return null;
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < getSlots.Length; i++)
        {
            if (getSlots[i].item.ID <= -1)
            {
                getSlots[i].UpdateSlot(_item, _amount);
                return getSlots[i];
            }
        }

        // inventory is full: set up what happens here later
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.GetItemObject) && item1.CanPlaceInSlot(item2.GetItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public void SwitchWeapon(Collider weaponSwapLocation)
    {
        GameObject x;

        if (equippedWeapon == WeaponType.Sword)
        {
            x = Instantiate(weapons[0].gameObject, new Vector3(weaponSwapLocation.transform.position.x - 0.5f,
            weaponSwapLocation.transform.position.y, weaponSwapLocation.transform.position.z + 1f), weapons[0].gameObject.transform.rotation);
            //x.GetComponent<GroundItem>().item.data.buffs = FindWeaponOnInventory().item.buffs;

        }
        else if (equippedWeapon == WeaponType.Bow)
        {
            x = Instantiate(weapons[1].gameObject, new Vector3(weaponSwapLocation.transform.position.x - 0.5f,
            weaponSwapLocation.transform.position.y, weaponSwapLocation.transform.position.z + 1f), weapons[1].gameObject.transform.rotation);
            //x.GetComponent<GroundItem>().item.data.buffs = FindWeaponOnInventory().item.buffs;                
        }
        else if (equippedWeapon == WeaponType.Staff)
        {
            x = Instantiate(weapons[2].gameObject, new Vector3(weaponSwapLocation.transform.position.x - 0.5f,
            weaponSwapLocation.transform.position.y, weaponSwapLocation.transform.position.z + 1f), weapons[2].gameObject.transform.rotation);
            //x.GetComponent<GroundItem>().item.data.buffs = FindWeaponOnInventory().item.buffs;                
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();

            foreach (InventorySlot slot in container.slots)
                if (slot.OnAfterUpdate != null) {
                    slot.OnAfterUpdate.Invoke(slot);
                }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container.Clear();
        equippedWeapon = WeaponType.None;
        equippedWeaponAttackType = WeaponAttackType.None;
    }
}
