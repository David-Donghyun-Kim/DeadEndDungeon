using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField]
    Transform player;
    void LateUpdate () {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
