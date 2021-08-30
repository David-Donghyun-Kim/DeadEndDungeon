using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCriticalDamageCommand : ICommand
{
    private GameObject target;
    private float damage, multiplier;
    
    public DealCriticalDamageCommand(GameObject target, float damage, float multiplier)
    {
        this.target = target;
        this.damage = damage;
        this.multiplier = multiplier;

    }

    public void Execute()
    {
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<AbstractEnemyController>().ChangeHealth((damage*multiplier));
            Debug.Log("Critical Hit!");
        }
    }
}
