using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CyclePictures : MonoBehaviour
{
    public float fadeFactor;
    public int cycleTime;
    int timer;
    int currentPictureIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentPictureIndex = Random.Range(0, transform.childCount);
        transform.GetChild(currentPictureIndex).gameObject.SetActive(true);
        timer = cycleTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        if(timer < 0)
        {
            timer = cycleTime;
            NextPicture();
        }

        
        if(timer < cycleTime / (fadeFactor * 1.0f))
        {
            transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color = 
            new Color(transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color.r,
            transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color.g,
            transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color.b, 
            timer / (cycleTime * 1.0f / fadeFactor));
        }

        if (timer > (cycleTime - (cycleTime / fadeFactor)))
        {
            transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color = 
            new Color(transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color.r,
            transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color.g,
            transform.GetChild(currentPictureIndex).gameObject.GetComponent<RawImage>().color.b, 
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

    void NextPicture()
    {
        transform.GetChild(currentPictureIndex).gameObject.SetActive(false);
        currentPictureIndex++;
        if(currentPictureIndex == transform.childCount) { currentPictureIndex = 0; }
        transform.GetChild(currentPictureIndex).gameObject.SetActive(true);
    }
}
