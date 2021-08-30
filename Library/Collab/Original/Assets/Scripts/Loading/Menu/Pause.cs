using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public KeyCode pauseGameKey;
    CursorLockMode previousCursorState;

    // Start is called before the first frame update
    void Start()
    {
        previousCursorState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseGameKey)) {
            if(Time.timeScale == 0.0f)
            {
                Time.timeScale = 1.0f;
                transform.GetChild(0).gameObject.SetActive(false);

                Cursor.lockState = previousCursorState;
            } else
            {
                Time.timeScale = 0.0f;
                transform.GetChild(0).gameObject.SetActive(true);
                previousCursorState = Cursor.lockState;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }
}
