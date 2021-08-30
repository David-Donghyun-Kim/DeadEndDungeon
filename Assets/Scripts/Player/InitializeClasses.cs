using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeClasses : MonoBehaviour
{
    public InventoryObject classHolder;
    [SerializeField]
    ItemObject[] classAttributes = new ItemObject[7];

    void Start()
    {
        GameObject newGameFlag = GameObject.Find("NewGameFlag");
        if (newGameFlag != null) {
            for (int i = 0; i < classAttributes.Length; i++) {
                Item classObj = new Item(classAttributes[i]);
                Debug.Log(classObj.name + ", " + classObj.name);
                classHolder.AddItem(classObj);
            }
            classHolder.Save();
            GameObject.Destroy(newGameFlag);
        } else {
            classHolder.Load();
        }
    }

    // void OnApplicationQuit() {
    //     classHolder.Clear();
    // }
}
