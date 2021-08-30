using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest Items List", menuName = "Inventory System/Items/Chest Items")]
public class ChestItems : ScriptableObject
{
    public GameObject[] lootableItems = new GameObject[7];
}
