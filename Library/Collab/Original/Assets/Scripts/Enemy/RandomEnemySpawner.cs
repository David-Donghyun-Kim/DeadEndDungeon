using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public EnemyPool EnemyManager;
    public GameObject player;

    public float speed = 3.0f;
    public float direction = 1;
    public bool xAxis = true;
    public double maxHealth = 5;

    // Start is called before the first frame update
    void Start()
    {
        int randomNum = Random.Range(0, enemyPrefabs.Count);
        GameObject createdEnemy = Instantiate(enemyPrefabs[randomNum], gameObject.transform.position, Quaternion.identity);

        if (createdEnemy != null)
        {
            EnemyManager.objectPool.Add(createdEnemy);
            createdEnemy.name = EnemyManager.objectPool.Count.ToString();

            AbstractEnemyController createdEnemyScript = createdEnemy.GetComponent<AbstractEnemyController>();
            createdEnemyScript.EnemyManager = EnemyManager;
            createdEnemyScript.player = player;

            createdEnemy.SetActive(false);
        }
    }
}
