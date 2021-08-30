using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchCharacter : MonoBehaviour
{
    public GameObject[] characterList;
    public InventoryObject classHolder;
    public InventoryObject townStatHolder;
    public InventoryObject basePlayerAttributes;

    public GameObject townSOInitializer;
    private int index;
    public bool isLeft = false;
	public GameObject player;

    void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");
        //Debug.Log("character int is " + index);
        
        characterList = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;

        foreach (GameObject characters in characterList)
            characters.SetActive(false);
        
        if (characterList[index])
            characterList[index].SetActive(true);

        if (basePlayerAttributes != null) {
            //if (townStatHolder != null) {
                
            if (townSOInitializer != null) {
                townSOInitializer.GetComponent<TownStatHolder>().SyncTownStats();
            }
            //} else {
                // basePlayerAttributes.getSlots[0].RemoveItem();
            basePlayerAttributes.getSlots[1].RemoveItem();
                // basePlayerAttributes.AddItem(townStatHolder.container.slots[0].item);
                // basePlayerAttributes.getSlots[0].UpdateSlot(basePlayerAttributes.getSlots[0].item, basePlayerAttributes.getSlots[0].amount);
            //}

            if (index <= 5)
                basePlayerAttributes.AddItem(classHolder.container.slots[index].item);
            else if (index == 6)
                basePlayerAttributes.AddItem(classHolder.container.slots[2].item);
            else if (index == 7)
                basePlayerAttributes.AddItem(classHolder.container.slots[4].item);
            else if (index == 8)
                basePlayerAttributes.AddItem(classHolder.container.slots[3].item);
            else if (index == 9)
                basePlayerAttributes.AddItem(classHolder.container.slots[1].item);
            else if (index == 10)
                basePlayerAttributes.AddItem(classHolder.container.slots[6].item);
            else if (index == 11)
                basePlayerAttributes.AddItem(classHolder.container.slots[5].item);
            characterList[index].SetActive(true);
        }
    }

    public void Toggle(bool isLeft) {
        Debug.Log("character int is " + index);
        //Debug.Log("character list is " + characterList);
        characterList[index].SetActive(false);

        if (isLeft) {
            index--;
            if (index < 0) 
                index = characterList.Length - 1;
        } else {
            index++;
            if (index == characterList.Length) 
                index = 0;
        }

        //if (townStatHolder != null) {
            
            // basePlayerAttributes.getSlots[0].RemoveItem();
        //} else {
        if (townSOInitializer != null) {
            townSOInitializer.GetComponent<TownStatHolder>().SyncTownStats();
        }
        basePlayerAttributes.getSlots[1].RemoveItem();
            // basePlayerAttributes.AddItem(townStatHolder.container.slots[0].item);
            // basePlayerAttributes.getSlots[0].UpdateSlot(basePlayerAttributes.getSlots[0].item, basePlayerAttributes.getSlots[0].amount);
        //}
   
        if (index <= 5)
		    basePlayerAttributes.AddItem(classHolder.container.slots[index].item);
        else if (index == 6)
            basePlayerAttributes.AddItem(classHolder.container.slots[2].item);
        else if (index == 7)
            basePlayerAttributes.AddItem(classHolder.container.slots[4].item);
        else if (index == 8)
            basePlayerAttributes.AddItem(classHolder.container.slots[3].item);
        else if (index == 9)
            basePlayerAttributes.AddItem(classHolder.container.slots[1].item);
        else if (index == 10)
            basePlayerAttributes.AddItem(classHolder.container.slots[6].item);
        else if (index == 11)
            basePlayerAttributes.AddItem(classHolder.container.slots[5].item);
        characterList[index].SetActive(true);
    }

    public void ConfirmButton (int sceneIndex) {
        PlayerPrefs.SetInt("CharacterSelected", index);
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnApplicationQuit(){
        PlayerPrefs.SetInt("CharacterSelected", 0);
    }
}
