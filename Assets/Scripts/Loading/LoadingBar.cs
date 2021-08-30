using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public float loadSpeed;
    public int sceneIndex;
    public int delay;
    int timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.GetComponent<RectTransform>().
        //transform.localScale = new Vector2(1.0f, 1.0f);
        //transform.localScale = new Vector2(Random.Range(0.0f, loadSpeed) + transform.localScale.x, transform.localScale.y);
        
        if(transform.localScale.x < 10.0f) { transform.localScale = new Vector2(Random.Range(0.0f, loadSpeed) + transform.localScale.x, transform.localScale.y); } else { timer--;  }

        if(timer < 0 ) { SceneManager.LoadScene(sceneIndex); }



    }
}
