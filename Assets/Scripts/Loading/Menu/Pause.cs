using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public KeyCode pauseGameKey;
    CursorLockMode previousCursorState;
    public GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        previousCursorState = CursorLockMode.Confined;
        GameObject statPanel = GameObject.Find("/Stats HUD");
        CanvasGroup cg = statPanel.transform.GetComponent<CanvasGroup>();
        cg.interactable= false;
        cg.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("InfoBoxes") != null) {
            return;
        }
        if(Input.GetKeyDown(pauseGameKey)) {
            if(Time.timeScale == 0.0f)
            {
                Time.timeScale = 1.0f;
                transform.GetChild(0).gameObject.SetActive(false);

                GameObject statPanel = GameObject.Find("/Stats HUD");
                CanvasGroup cg = statPanel.transform.GetComponent<CanvasGroup>();
                cg.interactable= false;
                cg.alpha = 0;

                if (crosshair != null) {
                    crosshair.SetActive(true);
                }
                
                Cursor.lockState = previousCursorState;
            } else
            {
                Time.timeScale = 0.0f;
                transform.GetChild(0).gameObject.SetActive(true);
                previousCursorState = Cursor.lockState;
                Cursor.lockState = CursorLockMode.Confined;
                GameObject statPanel = GameObject.Find("/Stats HUD");
                CanvasGroup cg = statPanel.transform.GetComponent<CanvasGroup>();
                cg.interactable= true;
                cg.alpha = 1;
        
                if (crosshair != null) {
                    crosshair.SetActive(false);
                }
            }
        }
    }
}
