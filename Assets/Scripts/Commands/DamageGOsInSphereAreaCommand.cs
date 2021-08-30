using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGOsInSphereAreaCommand : ICommand
{
    private SphereCollider col;
    private float damage;
    public DamageGOsInSphereAreaCommand(SphereCollider col, float damage)
    {
        this.col = col;
        this.damage = damage;
    }

    public void Execute()
    {
        UnityEngine.Collider[] GOs = Physics.OverlapSphere(col.transform.position, col.radius);

        foreach (Collider c in GOs)
        {
            float calcDmg = damage * (1 / (Vector3.Distance(c.transform.position, col.transform.position)));
            CommandInvoker.AddCommand(new DealDamageCommand(c.gameObject, calcDmg));
        }
    }
}
