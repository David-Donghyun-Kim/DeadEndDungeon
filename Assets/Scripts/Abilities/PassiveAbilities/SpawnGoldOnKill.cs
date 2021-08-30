using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGoldOnKill : MonoBehaviour
{
    private GameObject target;
    private int damage;
    public void OnAttack(object[] variables)
    {

        target = (GameObject)variables[0];
        damage = (int)variables[1];

        if (target.CompareTag("Enemy"))
        {
            if (target.GetComponent<AbstractEnemyController>().currentHealth - damage <= 0)
            {
                CommandInvoker.AddCommand(new GenerateGoldPileCommand(target.transform));
            }
        }
    }
}
