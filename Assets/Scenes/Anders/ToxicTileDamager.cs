using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicTileDamager : MonoBehaviour
{
    private GameObject entity;
    private int count = 0;
    [SerializeField] private int DamageDelay;
    [SerializeField] private int floorDamage;

    
    private void OnTriggerStay(Collider other)
    {
        entity = other.gameObject;

        if (count % DamageDelay == 0)
        {
            if (entity.CompareTag("Player"))
            {
                CommandInvoker.AddCommand(new DealDamageToPlayerCommand(floorDamage));
                Debug.Log("Dealing poison damage to player");
            }
        }
        ++count;
    }

    

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            count = 0;
        }
    }
}