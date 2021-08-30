using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<PlayerController>().moveSpeed = 1;
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") 
            other.gameObject.GetComponent<PlayerController>().moveSpeed = 7;
    }
}
