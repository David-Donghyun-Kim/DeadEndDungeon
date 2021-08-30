using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float fadeFactor;
    public int cycleTime;
    int timer;
    int currentPictureIndex;
	bool fading;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        if(timer < 0)
        {
            
        }

        
        if(!fading)
        {
			
			
            gameObject.GetComponent<RawImage>().color = 
            new Color(gameObject.GetComponent<RawImage>().color.r,
            gameObject.GetComponent<RawImage>().color.g,
            gameObject.GetComponent<RawImage>().color.b, 
            timer / (cycleTime * 1.0f / fadeFactor));
        }

        if (fading)
        {
            gameObject.GetComponent<RawImage>().color = 
            new Color(gameObject.GetComponent<RawImage>().color.r,
            gameObject.GetComponent<RawImage>().color.g,
            gameObject.GetComponent<RawImage>().color.b, 
            (cycleTime - timer) / (cycleTime / fadeFactor));
        }/*

        if (timer < cycleTime / (fadeFactor * 1.0f))
        {
            gameObject.GetComponent<RawImage>().color =
            new Color(gameObject.GetComponent<RawImage>().color.r,
            gameObject.GetComponent<RawImage>().color.g,
            gameObject.GetComponent<RawImage>().color.b,
            timer / (cycleTime * 1.0f / fadeFactor));
        }

        if (timer > (cycleTime - (cycleTime / fadeFactor)))
        {
            gameObject.GetComponent<RawImage>().color =
            new Color(gameObject.GetComponent<RawImage>().color.r,
            gameObject.GetComponent<RawImage>().color.g,
            gameObject.GetComponent<RawImage>().color.b,
            (cycleTime - timer) / (cycleTime / fadeFactor));
        }
*/
        timer--;
    }
	
	public void FadeOut() {
		fading = true;
		timer = cycleTime;
	}
	
	public void FadeIn() {
		fading = false;
		timer = cycleTime;
	}

    
}
