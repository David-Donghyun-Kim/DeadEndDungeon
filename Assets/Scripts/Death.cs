using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    CursorLockMode previousCursorState;
    public GameObject crosshair;
    public InventoryObject inventory; // for clearing

    public void Die()
    {
        previousCursorState = CursorLockMode.Confined;
        transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        previousCursorState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Confined;
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
        inventory.Clear();
        inventory.Save();
    }
}
