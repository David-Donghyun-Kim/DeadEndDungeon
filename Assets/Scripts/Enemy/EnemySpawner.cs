using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public EnemyPool EnemyManager;
    public GameObject parent;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject createdEnemy = Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);
        
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
