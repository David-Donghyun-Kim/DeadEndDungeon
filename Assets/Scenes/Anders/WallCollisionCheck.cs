using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionCheck : MonoBehaviour
{     
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall") {
            Debug.Log("Deleted " + gameObject.name + " Hit wall: " + other.gameObject.tag);
            Destroy(gameObject);
        } else if (other.gameObject.tag == "Prop") {
            Debug.Log("Deleted " + other.gameObject.name + " Hit prop: " + other.gameObject.tag);
            Destroy(other.gameObject);
        }
    }
}
