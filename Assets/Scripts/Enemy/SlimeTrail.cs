using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrail : MonoBehaviour
{
    public GameObject trailCollider;
    int trailLength = 5;
    float spawnInterval = 0.5f;
    float spawnTimer = 0;

    void Update () {
        if (gameObject.GetComponent<MeleeEnemy>().state == AbstractEnemyController.EnemyState.Following) {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > spawnInterval) {
                GameObject x = Instantiate(trailCollider, new Vector3(transform.position.x, transform.localPosition.y, transform.position.z), trailCollider.transform.rotation);
                spawnTimer = 0;
            } 
        }
    }
}
