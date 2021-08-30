using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagement : MonoBehaviour
{

    public GameObject oldMenu;
    public GameObject newMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void changeMenu()
    {
        oldMenu.SetActive(false);
        newMenu.SetActive(true);
    }
}
