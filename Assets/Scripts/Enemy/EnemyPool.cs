using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EnemyPool : MonoBehaviour
{
    public GameObject player;
    public float playerSightRange;
    float enemySightRange;
    public List<GameObject> objectPool;
    public List<GameObject> inPlayerRange { get { return playerRange; } }
    private List<GameObject> playerRange;
    public List<GameObject> inEnemyRange { get { return enemyRange; } }
    private List<GameObject> enemyRange;

    // Start is called before the first frame update
    void Awake()
    {
        objectPool = new List<GameObject>();
        playerRange = new List<GameObject>();
        enemyRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject thing in objectPool)
        {
            if (thing != null)
            {
                enemySightRange = thing.GetComponent<AbstractEnemyController>().enemyData.sightRange;
                if (Vector3.Distance(player.transform.position, thing.transform.position) <= playerSightRange)
                {
                    if (!playerRange.Contains(thing))
                    {
                        playerRange.Add(thing);
                    }
                    thing.SetActive(true);

                    if (Vector3.Distance(player.transform.position, thing.transform.position) <= enemySightRange && !enemyRange.Contains(thing))
                    {
                        enemyRange.Add(thing);
                    }
                }
                if (Vector3.Distance(player.transform.position, thing.transform.position) > enemySightRange && enemyRange.Contains(thing))
                {
                    enemyRange.Remove(thing);
                }
                if (!thing.activeInHierarchy || Vector3.Distance(player.transform.position, thing.transform.position) > playerSightRange)
                {
                    if (playerRange.Contains(thing))
                    {
                        playerRange.Remove(thing);
                    }
                    thing.SetActive(false);

                    if (enemyRange.Contains(thing))
                    {
                        enemyRange.Remove(thing);
                    }
                }
            }
        }

        objectPool = objectPool.Where(item => item != null).ToList();
    }
}
