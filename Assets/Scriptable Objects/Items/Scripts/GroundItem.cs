using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;


    void Start() {
        gameObject.GetComponent<Outline>().enabled = false;
    }

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
// #if UNITY_EDITOR
//         // Debug.Log(GetComponentInChildren<SpriteRenderer>());
//         // GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        
//         // EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
// #endif
    }
}
