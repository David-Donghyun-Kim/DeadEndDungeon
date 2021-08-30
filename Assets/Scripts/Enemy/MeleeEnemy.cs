using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : AbstractEnemyController
{
    [SerializeField]
    float attackAnimationDelay;

    void Start()
    {
        baseStart();
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
    }

    void Update()
    {
        //gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        baseUpdate();
    }

    IEnumerator ColliderDisable() {
        yield return new WaitForSeconds(1.4f);
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
    }

    override protected Vector3 Attack()
    {
        if (attackCooldown >= enemyData.attackRefreshMin)
        {
            //TODO do the attack things
            soundManager.playAttackSound();
            Debug.Log("Attacking now");
            animator.SetTrigger("attacking");
            gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
            attackCooldown = 0;
            StartCoroutine(ColliderDisable());

        }
        
        return FollowPlayer();
    }
}
