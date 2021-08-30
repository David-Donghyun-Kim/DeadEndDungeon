using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestZone_Lift : MonoBehaviour
{
    public bool goUp = true;
    void Update()
    {
        if (transform.position.y >= 20)
            goUp = false;
        if (transform.position.y < -1)
            goUp = true;

        if (goUp)
            transform.position += Vector3.up * 5f * Time.deltaTime;
        else
            transform.position += Vector3.down * 5f * Time.deltaTime;
    }

}
