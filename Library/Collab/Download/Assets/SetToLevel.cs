using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetToLevel : MonoBehaviour
{
	public GameObject dg;
    private bool onTownScreen = false;
	TextMeshProUGUI myText;
	
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
        if (dg == null)
            onTownScreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (onTownScreen) {
            myText.SetText("N/A");
        } else {
            string floor = "Floor: " + dg.GetComponent<LevelSpawner>().GetFloor();
            myText.SetText(floor);
        }
    }
}
