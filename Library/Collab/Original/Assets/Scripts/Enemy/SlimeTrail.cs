using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrail : MonoBehaviour
{
    public GameObject trailCollider;
    //public GameObject[] colliderList = new GameObject[5];
    int trailLength = 5;
    int numColliders = 0;
    float spawnInterval = 0.5f;
    float spawnTimer = 0;
  
    void Update () {
        if (gameObject.GetComponent<MeleeEnemy>().state == AbstractEnemyController.EnemyState.Following) {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > spawnInterval) {

                // if (numColliders >= trailLength) {
                //     GameObject.Destroy(colliderList[2]);
                //     numColliders--;
                // }

                GameObject x = Instantiate(trailCollider, new Vector3(transform.position.x, 0.2f, transform.position.z), trailCollider.transform.rotation);

                //colliderList[numColliders] = x;
                //numColliders++;

                spawnTimer = 0;
            } 
        }
    }
}
