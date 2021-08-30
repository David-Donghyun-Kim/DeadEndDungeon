using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    public KeyCode tutorialGameKey;
    CursorLockMode previousCursorState;
    public GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        //previousCursorState = CursorLockMode.Confined;

        Time.timeScale = 0.0f;
        transform.GetChild(0).gameObject.SetActive(true);
        // previousCursorState = Cursor.lockState;
        // Cursor.lockState = CursorLockMode.Confined;

        if (crosshair != null) {
            crosshair.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Paused") != null) {
            return;
        }
        if(Input.GetKeyDown(tutorialGameKey)) {
            if(Time.timeScale == 0.0f)
            {
                Time.timeScale = 1.0f;
                transform.GetChild(0).gameObject.SetActive(false);
                if (crosshair != null) {
                    crosshair.SetActive(true);
                }
                
                //Cursor.lockState = previousCursorState;
            } else
            {
                Time.timeScale = 0.0f;
                transform.GetChild(0).gameObject.SetActive(true);
                //previousCursorState = Cursor.lockState;
                //Cursor.lockState = CursorLockMode.Confined;
        
                if (crosshair != null) {
                    crosshair.SetActive(false);
                }
            }
        }
    }
}
