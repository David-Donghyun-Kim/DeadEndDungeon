using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private int health;
    float hitTime = 1.5f;
    float hitTimer = 0;
    bool canHit = true;

    // Start is called before the first frame update
    void Start()
    {
        health = 20;
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer += Time.deltaTime;
 
        if(hitTimer > hitTime)
            canHit = true; 
    }

    void OnTriggerEnter(Collider other)
    {
        
    }

    public void TakeDamage(int damageReceived)
    {
        if(!canHit)
            return;

        health = health - damageReceived;
        Debug.Log("Enemy took damage - health is " + health);

        hitTimer = 0;
    }
}