using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    float DestroyTime = 1.5f;

    //public Vector3 offset = new Vector3(0, 2, 0);
    [SerializeField]
    public GameObject GOToFace;

    void Start()
    {
        Vector3 offset = new Vector3(0, 0, 0);
        if (GetComponentInParent<BoxCollider>() != null)
            offset = new Vector3(0f, (float)(GetComponentInParent<BoxCollider>().center.y * 2.2), 0f);
        else
            offset = new Vector3(0, 2, 0);
        transform.LookAt(Camera.main.transform); 
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        transform.localPosition += offset;
        Destroy(gameObject, DestroyTime);
    }

    void LateUpdate()
     {
        transform.LookAt(GOToFace.transform); 
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
     }
}
