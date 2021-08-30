using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Item
{
    public string name;
    public int ID = -1;
    public ItemBuff[] buffs;
    public WeaponAttackType attackType;
    public WeaponType weaponType;
    public int amount;
    public Item()
    {
        name = "";
        ID = -1;
    }

    public Item(ItemObject item)
    {
        name = item.name;
        ID = item.data.ID;
        amount = item.data.amount;
        weaponType = item.data.weaponType;
        buffs = new ItemBuff[item.data.buffs.Length];

        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }

        attackType = (WeaponAttackType)Random.Range(0, 5);
        if (item.name == "Staff" && attackType == WeaponAttackType.Poison)
            attackType = (WeaponAttackType)Random.Range(0, 4);
    }
}