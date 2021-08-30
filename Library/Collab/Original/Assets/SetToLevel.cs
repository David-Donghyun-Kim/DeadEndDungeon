using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetToLevel : MonoBehaviour
{
	public GameObject dg;
	TextMeshProUGUI myText;
	
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

		string floor = "Floor: " + dg.GetComponent<LevelSpawner>().GetFloor();
        myText.SetText(floor);
    }
}
