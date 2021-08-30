using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetToStats : MonoBehaviour
{
	TextMeshProUGUI myText;
	
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

		string floor = "Dex: " + PlayerPrefs.GetFloat("Dexterity") + 
			"\nInt: " + PlayerPrefs.GetFloat("Intellect") + 
			"\nStr: " + PlayerPrefs.GetFloat("Strength") + 
			"\nSta: "+ PlayerPrefs.GetFloat("Stamina");
        myText.SetText(floor);
    }
}
