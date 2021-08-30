using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Following,
        MeleeAttacking,
        ProjectileAttacking,
        Dead
    }
    
    public EnemyPool EnemyManager;

    public SO_TestEnemy enemyData;

    Rigidbody rb;

    Animator animator;

    public GameObject player;

    public EnemyState state;
    Vector3 seekDirection;
    Vector3 prevPos;

    public double currentHealth;

    public double health { get { return currentHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = enemyData.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO add attacking state conditions
        if (state != EnemyState.Dead)
        {
            if (EnemyManager.inEnemyRange.Contains(gameObject))
            {
                state = EnemyState.Following;
            }
            else
            {
                state = EnemyState.Patrolling;
            }
        }
        
        //Debug.Log("Enemy health is " + health);

        prevPos = transform.position;

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Damaging enemy.");
            ChangeHealth(-1);
        }
    }


    void FixedUpdate()
    {
        Vector3 position = transform.position;

        switch (state)
        {
            case EnemyState.Patrolling:
                position = Patrol();
                seekDirection = (transform.position - prevPos).normalized;
                break;
            case EnemyState.Following:
                position = FollowPlayer();
                break;
            case EnemyState.MeleeAttacking:
                position = MeleeAttack();
                // code block
                break;
            case EnemyState.ProjectileAttacking:
                position = ProjectileAttack();
                // code block
                break;
            case EnemyState.Dead:
                // currently this should do nothing
                break;
            default:
                // code block
                break;
        }

        if (Math.Abs(transform.position.x - player.transform.position.x) >= 0.1 || Math.Abs(transform.position.z - player.transform.position.z) >= 0.1)
        {
            transform.position = position;

            seekDirection = (transform.position - prevPos).normalized;
            seekDirection.y = 0f;

            transform.rotation = Quaternion.LookRotation(seekDirection);

            animator.SetFloat("speed", enemyData.speed);
        }
        else
        {
            animator.SetFloat("speed", 0f);
        }
    }


    public void ChangeHealth(double amount)
    {
        currentHealth += amount;
        if (health > enemyData.maxHealth)
        {
            currentHealth = enemyData.maxHealth;
        }
        else if (health <= 0)
        {
            currentHealth = 0;
            state = EnemyState.Dead;
            Died();
        }
        else if (amount <= 0)
        {
            animator.SetTrigger("damaged");
        }
        
    }

    void Died()
    {
        animator.SetTrigger("died");
        EnemyManager.objectPool.Remove(gameObject);
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Floor")
        {
            enemyData.direction = -enemyData.direction;
        }
    }

    Vector3 Patrol()
    {
        Vector3 position = rb.position;

        if (enemyData.xAxis)
        {
            position.x += Time.deltaTime * enemyData.speed * enemyData.direction;
        }
        else
        {
            position.z += Time.deltaTime * enemyData.speed * enemyData.direction;
        }

        return position;
    }

    Vector3 FollowPlayer()
    {
        Vector3 position = rb.position;

        if (Vector3.Distance(player.transform.position, transform.position) > 0)
        {
            seekDirection = new Vector3((player.transform.position.x - transform.position.x), 0, (player.transform.position.z - transform.position.z));
            seekDirection.Normalize();
        }

        position += Time.deltaTime * enemyData.speed * seekDirection;

        return position;
    }

    Vector3 MeleeAttack()
    {
        Vector3 position = rb.position;

        //TODO do the attack things

        return FollowPlayer();
    }
    Vector3 ProjectileAttack()
    {
        Vector3 position = rb.position;

        //TODO do the attack things

        return position;
    }

}
