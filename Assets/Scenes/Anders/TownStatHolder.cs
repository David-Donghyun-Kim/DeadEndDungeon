using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownStatHolder : MonoBehaviour
{
    [SerializeField]
    public ItemObject townStats;
    public InventoryObject townStatHolder;
    public InventoryObject basePlayerAttributes;

    void Start() {
        townStatHolder.Load();
    }

    public void UpdateTownStats(int dexterity, int intellect, int strength, int stamina) {
        // Debug.Log("Updating town stats via TownStatHolder");
        if (townStatHolder != null) {
            if (townStatHolder.container.slots[0].item.ID != -1) {
                townStatHolder.getSlots[0].RemoveItem();
            } 

            ItemBuff dex = new ItemBuff(dexterity, dexterity);
            dex.attribute = BuffType.Dexterity;
            ItemBuff inte = new ItemBuff(intellect, intellect);
            inte.attribute = BuffType.Intellect;
            ItemBuff str = new ItemBuff(strength, strength);
            str.attribute = BuffType.Strength;
            ItemBuff stam = new ItemBuff(stamina, stamina);
            stam.attribute = BuffType.Stamina;
            
            townStats.data.buffs[0] = dex;
            townStats.data.buffs[1] = str;
            townStats.data.buffs[2] = stam;
            townStats.data.buffs[3] = inte;

            // foreach (ItemBuff buff in townStats.data.buffs)
            // {
            //     Debug.Log("New " + buff.attribute + " is " + buff.value);
            // }

            Item statObj = new Item(townStats);
            townStatHolder.AddItem(statObj);
            townStatHolder.Save();
        } 
    }


    public void SyncTownStats() {
        if (basePlayerAttributes != null) {
            // probably dont need to be removing it and adding it back, or saving. im doing it just in case anyway
            basePlayerAttributes.getSlots[0].RemoveItem();
            basePlayerAttributes.AddItem(townStatHolder.container.slots[0].item);
            basePlayerAttributes.getSlots[0].UpdateSlot(basePlayerAttributes.getSlots[0].item, basePlayerAttributes.getSlots[0].amount);
            basePlayerAttributes.Save();
        }
    }
}
