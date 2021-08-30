using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************************************************************
 * Yoinked some framework from Chest, so it works pretty similarly but without having to instantiate
 * an actual item in the game world.
 */
public class ShopManager : MonoBehaviour
{
    public InventoryObject inventory;
    public ChestItems itemPrefabs; // database
    private int[] generatedItems = new int[2]; // IDs of items that are to go in the shop

    // IDs
    private int weaponID;
    private int itemID;

    // costs
    private int weaponCost = 500;
    private int itemCost = 500;

    // UI
    public GameObject grid;
    private BuildingGridController gridScript;

    // Start is called before the first frame update
    void Start()
    {
        if ((grid = GameObject.Find("/Grid")) == null)
        {
            Debug.Log("Error: BuildingController could not find building grid.");
        }
        gridScript = grid.GetComponent<BuildingGridController>();
        GenerateNewWeapon();
        GenerateNewItem();
    }

    private void GenerateNewWeapon()
    {
        weaponID = Random.Range(0, 3);
        generatedItems[0] = weaponID;
        Debug.Log("Generated weapon with ID " + weaponID);
    }

    private void GenerateNewItem()
    {
        itemID = Random.Range(4, 7);
        generatedItems[1] = itemID;
        if (itemID == 5) generatedItems[1] = Random.Range(6, 7); // gold, just choose something else
        Debug.Log("Generated item with ID " + itemID);
    }

    public void PurchaseWeapon()
    {
        if (gridScript.currentGold >= weaponCost)
        {
            AddToInventory(weaponID, weaponCost);
            GenerateNewWeapon();
        }
        else
        {
            gridScript.DisplayActionText("You don't have enough gold for this weapon...");
        }
    }

    public void PurchaseItem()
    {
        if (gridScript.currentGold >= itemCost)
        {
            AddToInventory(itemID, itemCost);
            GenerateNewItem();
        }
        else
        {
            gridScript.DisplayActionText("You don't have enough gold for this item...");
        }
    }

    private void AddToInventory(int id, int cost)
    {
        gridScript.UpdateGold(cost);
        GameObject itemGO = Instantiate(itemPrefabs.lootableItems[id]);
        Item _item = new Item(itemGO.GetComponent<GroundItem>().item);
        gridScript.DisplayActionText("Purchased a " + _item.name + "!");
        inventory.AddItem(_item);
        Destroy(itemGO);
    }
}
