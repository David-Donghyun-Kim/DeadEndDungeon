using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    public Transform firePos;
    public RaycastHit aimTarget;
    public GameObject arrowPrefab;
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, 
            Camera.main.transform.TransformDirection(Vector3.forward) * 200.0f, 
            Color.green);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward),
            out aimTarget, 200.0f))
        {
            Instantiate(arrowPrefab, firePos);
        }
    }
}