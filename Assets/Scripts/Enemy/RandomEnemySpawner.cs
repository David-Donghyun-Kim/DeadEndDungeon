using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : EnemySpawner
{
    public List<GameObject> enemyPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        int randomNum = Random.Range(0, enemyPrefabs.Count);
        GameObject createdEnemy = Instantiate(enemyPrefabs[randomNum], gameObject.transform.position, Quaternion.identity);

        if (createdEnemy != null)
        {
            EnemyManager.objectPool.Add(createdEnemy);
            createdEnemy.transform.parent = parent.transform;
            createdEnemy.name = EnemyManager.objectPool.Count.ToString();

            AbstractEnemyController createdEnemyScript = createdEnemy.GetComponent<AbstractEnemyController>();
            createdEnemyScript.EnemyManager = EnemyManager;
            createdEnemyScript.player = player;

            createdEnemy.SetActive(false);

            Destroy(gameObject);
        }
    }
}
