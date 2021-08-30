using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrailDespawn : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 10);
    }
}
