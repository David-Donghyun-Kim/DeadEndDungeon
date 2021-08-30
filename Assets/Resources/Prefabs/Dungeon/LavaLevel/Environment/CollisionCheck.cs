using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
       if  (other.gameObject.tag == "Chest" || other.gameObject.tag == "Portal") {
            Destroy(gameObject);
        } else if (other.gameObject.tag == "Prop") {
            Destroy(other.gameObject);
        }
    }
}
