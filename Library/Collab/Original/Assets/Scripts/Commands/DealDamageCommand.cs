using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageCommand : ICommand
{
    private GameObject target;
    private float damage;

    public DealDamageCommand(GameObject target, float damage)
    {
        this.target = target;
        this.damage = damage;

    }

    public void Execute()
    {
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<AbstractEnemyController>().ChangeHealth(damage);
        }
    }
}
