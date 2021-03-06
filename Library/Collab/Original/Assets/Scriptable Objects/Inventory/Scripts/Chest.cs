using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool open;
    public ChestItems itemPrefabs; // basically chest database
    private int[] generatedItems = new int[2]; // IDs of items that are to go in the specific chest instance
    private int itemOneID;
    private int itemTwoID;
    float updateTime = 1.5f;
    float updateTimer = 0;
    bool canUpdateChestData = true;

    void Start()
    {
        gameObject.GetComponent<Animator>().SetBool("open", false);
        itemOneID = Random.Range(0, 6);  // get ID of two random items
        itemTwoID = Random.Range(0, 6);
        generatedItems[0] = itemOneID;
        generatedItems[1] = itemTwoID;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            gameObject.GetComponent<Animator>().SetBool("open", true);

            if (generatedItems[0] != -1)
            {
                GameObject x = Instantiate(itemPrefabs.lootableItems[itemOneID], new Vector3(transform.position.x - 0.4f, 
                itemPrefabs.lootableItems[itemOneID].transform.position.y, transform.position.z), itemPrefabs.lootableItems[itemOneID].transform.rotation);

                x.transform.SetParent(gameObject.transform);
            }

            if (generatedItems[1] != -1)
            {
                GameObject y = Instantiate(itemPrefabs.lootableItems[itemTwoID], new Vector3(transform.position.x + 0.4f, 
                itemPrefabs.lootableItems[itemTwoID].transform.position.y, transform.position.z),  itemPrefabs.lootableItems[itemTwoID].transform.rotation);

                y.transform.SetParent(gameObject.transform);
            }
        }
    }

    // serves as a delay for OnTriggerStay, since OnTriggerStay can be expensive
    void Update()
    {
        updateTimer += Time.deltaTime;
 
        if(updateTimer > updateTime)
            canUpdateChestData = true; 
    }

    private void OnTriggerStay(Collider other)
    {
        // delay 
        if(!canUpdateChestData)
            return;

        if (other.gameObject.tag == "Player") {
            int children = gameObject.transform.childCount;

            if (children == 3) { // player looted one item
                int itemRemainingInChest = gameObject.GetComponent<Transform>().GetChild(2).GetComponent<GroundItem>().item.data.ID;
                
                if (generatedItems[0] != itemRemainingInChest) // dont want to respawn an item that was looted
                    generatedItems[0] = -1;
                else
                    generatedItems[1] = -1;

            } else if (children == 2) {
                generatedItems[0] = -1;
                generatedItems[1] = -1;
            }
        }

        updateTimer = 0;
    }

    private void OnTriggerExit(Collider other)
    {   
        if (other.gameObject.tag == "Player") {
            bool firstItemNotYetLooted = generatedItems[0] != -1;
            bool secondItemLootedNotYetLooted = generatedItems[1] != -1; 
            
            if (secondItemLootedNotYetLooted && firstItemNotYetLooted) {
                Destroy(GetComponent<Transform>().GetChild(3).gameObject);
                Destroy(GetComponent<Transform>().GetChild(2).gameObject);
                gameObject.GetComponent<Animator>().SetBool("open", false);

            } else if (secondItemLootedNotYetLooted || firstItemNotYetLooted) {
                Destroy(GetComponent<Transform>().GetChild(2).gameObject);
                gameObject.GetComponent<Animator>().SetBool("open", false);

            } else { // both were looted
                gameObject.GetComponent<Animator>().SetBool("open", true);
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}