using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : AbstractEnemyController
{
    public GameObject projectile;
    public Queue<GameObject> activeProjectiles;

    void Start()
    {
        baseStart();
        activeProjectiles = new Queue<GameObject>();
    }

    void Update()
    {
        baseUpdate();

        while (activeProjectiles.Count > 5)
        {
            GameObject temp = activeProjectiles.Dequeue();
            Destroy(temp);
        }
    }

    protected override Vector3 FollowPlayer()
    {
        if (state != EnemyState.Dead)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 0)
            {
                seekDirection = new Vector3((player.transform.position.x - transform.position.x), 0, (player.transform.position.z - transform.position.z));
                seekDirection.Normalize();
                if (seekDirection != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(seekDirection);
            }
        }

        return transform.position;
    }

    override protected Vector3 Attack()
    {
        //TODO do the attack things
        Vector3 position = new Vector3(0,0,0);
        position = FollowPlayer();
        if (attackCooldown >= enemyData.attackRefreshMin)
        {
            Debug.Log("Attacking now");
            //animator.SetFloat("speed", 0f);
            animator.SetTrigger("attacking");
            attackCooldown = 0;

            soundManager.playAttackSound();

            GameObject createdProjectile = Instantiate(projectile, gameObject.transform.position + (seekDirection + new Vector3(0f, 1f, 0f)), Quaternion.identity);
            Debug.Log("proj created " + createdProjectile.gameObject.name);
            createdProjectile.tag = "EnemyAttack";
            createdProjectile.GetComponent<EnemyProjectile>().travelDirection = seekDirection;
            createdProjectile.GetComponent<EnemyProjectile>().damage = enemyData.baseAttackDamage;
            activeProjectiles.Enqueue(createdProjectile);
        }

        return position;
    }
}
