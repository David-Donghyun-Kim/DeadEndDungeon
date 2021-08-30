using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Following,
        Attacking,
        Dead
    }
    
    public EnemyPool EnemyManager;

    public SO_Enemy enemyData;
    protected float attackCooldown;

    protected Rigidbody rb;

    protected Animator animator;

    public GameObject player;

    public EnemyState state;
    protected Vector3 seekDirection;
    protected Vector3 prevPos;
    public float currentDirection;

    public double currentHealth;
    protected float hitTime = 2.5f;
    protected float hitTimer = 0;
    protected bool canHit = true;

    public double health { get { return currentHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        baseStart();
    }

    protected void baseStart()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = enemyData.maxHealth;
        currentDirection = enemyData.startingDirection;
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
    }

    protected void baseUpdate()
    {
        if (state != EnemyState.Dead)
        {
            hitTimer += Time.deltaTime;

            if (hitTimer > hitTime)
            {
                canHit = true;
            }

            attackCooldown += Time.deltaTime;

            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= enemyData.attackRange && state != EnemyState.Attacking)
            {
                if (attackCooldown >= enemyData.attackRefreshMin)
                {
                    state = EnemyState.Attacking;
                }
            }
            else if (EnemyManager.inEnemyRange.Contains(gameObject))
            {
                state = EnemyState.Following;
            }
            else
            {
                state = EnemyState.Patrolling;
            }
            //Debug.Log("Enemy health is " + health);
            prevPos = transform.position;
            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("Damaging enemy.");
                ChangeHealth(-1);
            }
        }
    }

    void FixedUpdate()
    {
        if (state != EnemyState.Dead)
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
                case EnemyState.Attacking:
                    position = Attack();
                    // code block
                    break;
                case EnemyState.Dead:
                    // this should do nothing
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

                if (seekDirection != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(seekDirection);

                animator.SetFloat("speed", enemyData.speed);
            }
            else
            {
                animator.SetFloat("speed", 0f);
            }
        }
    }


    public void ChangeHealth(double amount)
    {
        if (amount.Equals(0) || (amount < 0 && !canHit))
        {
            return;
        }

        currentHealth += amount;
        if (health > enemyData.maxHealth)
        {
            currentHealth = enemyData.maxHealth;
        }
        else if (health <= 0)
        {
            currentHealth = 0;
            if (state != EnemyState.Dead)
            {
                state = EnemyState.Dead;
                Died();
            }
        }
        else if (amount <= 0)
        {
            animator.SetTrigger("damaged");
        }
        Debug.Log("Enemy health updated - health is " + currentHealth);

        hitTimer = 0;
    }

    void Died()
    {
        Debug.Log(gameObject.name + " has died.");
        animator.SetTrigger("died");
        EnemyManager.objectPool.Remove(gameObject);
        gameObject.AddComponent(Type.GetType("Outline"));
        gameObject.layer = LayerMask.NameToLayer("Loot");
        gameObject.GetComponent<Outline>().OutlineColor = new Color(255, 0, 0);

        //Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (state != EnemyState.Dead)
        {
            if (other.gameObject.tag != "Floor" && other.gameObject.tag != "Weapon")
            {
                currentDirection = -currentDirection;
            }
            if (other.gameObject.tag == "Weapon")
            {
                Debug.Log("Enemy: " + gameObject.name + " is being hit by" + other.gameObject.name);

                PlayerCurrentWeapon playerWeapon = player.GetComponent<PlayerCurrentWeapon>();
                int damage = 0;
                if (playerWeapon != null)
                {
                    if (playerWeapon.weaponType == WeaponType.Sword)
                    {
                        damage = playerWeapon.weapons[0].GetComponent<WeaponManager>().Attack(this);
                    }
                    else if (playerWeapon.weaponType == WeaponType.Bow)
                    {
                        damage = playerWeapon.weapons[2].GetComponent<WeaponManager>().Attack(this);
                    }
                }
                player.BroadcastMessage("OnAttack", PackageAttackInfo(gameObject, damage), SendMessageOptions.DontRequireReceiver);
                CommandInvoker.AddCommand(new DealDamageCommand(gameObject, damage));
                //ChangeHealth(damage);
            }
        }
    }

    virtual protected Vector3 Patrol()
    {
        Vector3 position = rb.position;

        if (state != EnemyState.Dead)
        {
            if (enemyData.xAxis)
            {
                position.x += Time.deltaTime * enemyData.speed * currentDirection;
            }
            else
            {
                position.z += Time.deltaTime * enemyData.speed * currentDirection;
            }
        }

        return position;
    }

    virtual protected Vector3 FollowPlayer()
    {
        Vector3 position = rb.position;

        if (state != EnemyState.Dead)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 0)
            {
                seekDirection = new Vector3((player.transform.position.x - transform.position.x), 0, (player.transform.position.z - transform.position.z));
                seekDirection.Normalize();
            }

            position += Time.deltaTime * enemyData.speed * seekDirection;
        }
        return position;
    }

    virtual protected Vector3 Attack()
    {
        Vector3 position = rb.position;

        if (state != EnemyState.Dead)
        {
            animator.SetTrigger("attacking");
            gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
            //TODO do the attack things 
        }

        return position;
    }

    private object[] PackageAttackInfo(GameObject obj, int damage)
    {
        var arr = new object[2];
        arr[0] = obj;
        arr[1] = damage;

        return arr;
    }

}
