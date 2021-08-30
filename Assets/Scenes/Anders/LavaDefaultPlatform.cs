using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDefaultPlatform : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject volcanoPrefab;
    public GameObject smallestPlatformPrefab;
    void Start()
    {
        GameObject x;
        int value = Random.Range(0, 10);
        if (value < 3) {
            x = Instantiate(volcanoPrefab, new Vector3(-0.575f, 0f, -0.325f), transform.rotation);
            x.transform.SetParent(gameObject.transform);
            x.transform.localPosition = new Vector3(-0.575f, 0f, -0.325f);
        } else {
            x = Instantiate(smallestPlatformPrefab, new Vector3(-0.575f, 0f, -0.325f), transform.rotation);
            x.transform.SetParent(gameObject.transform);
            x.transform.localPosition = new Vector3(-0.575f, 0f, -0.325f);
        }

        GameObject y;
        int secondValue = Random.Range(0, 10);
        if (secondValue == 0) {
            y = Instantiate(volcanoPrefab, new Vector3(0.465f, 0f, 0.515f), transform.rotation);
            y.transform.SetParent(gameObject.transform);
            y.transform.localPosition = new Vector3(0.465f, 0f, 0.515f);
        } else if (secondValue < 3) {
            y = Instantiate(smallestPlatformPrefab, new Vector3(0.465f, 0f, 0.515f), transform.rotation);
            y.transform.SetParent(gameObject.transform);
            y.transform.localPosition = new Vector3(0.465f, 0f, 0.515f);
        } else {
            // no platform
        }
        
    }
}
