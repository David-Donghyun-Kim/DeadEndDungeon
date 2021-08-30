using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChecker : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        playerController.jumpChecker = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerController.jumpChecker = false;
    }
}
