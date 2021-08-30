using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnEnemyCorpseCommand : ICommand
{
    private GameObject enemyToDespawn;

    public DespawnEnemyCorpseCommand(GameObject enemyToDespawn)
    {
        this.enemyToDespawn = enemyToDespawn;
    }
    public void Execute() 
    {
        if (enemyToDespawn.CompareTag("Enemy"))
        {
            if (enemyToDespawn.GetComponent<AbstractEnemyController>().state == AbstractEnemyController.EnemyState.Dead)
            {
                Object.Destroy(enemyToDespawn);
            }
        }
    }
}
