using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinDefaultPlatform : MonoBehaviour
{
    public GameObject smallestPlatformPrefab;
    void Start()
    {
        GameObject y;
        int secondValue = Random.Range(0, 10);
        if (secondValue < 4) {
            y = Instantiate(smallestPlatformPrefab, new Vector3(0.465f, 0f, 0.515f), transform.rotation);
            y.transform.SetParent(gameObject.transform);
            y.transform.localPosition = new Vector3(0.465f, 0f, 0.515f);
        }
        // } else if (secondValue < 3) {
        //     y = Instantiate(smallestPlatformPrefab, new Vector3(0.465f, 0f, 0.515f), transform.rotation);
        //     y.transform.SetParent(gameObject.transform);
        //     y.transform.localPosition = new Vector3(0.465f, 0f, 0.515f);
        // } else {
        //     // no platform
        // }
        
    }
}
