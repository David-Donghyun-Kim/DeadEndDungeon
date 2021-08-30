using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

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

    protected EnemySoundManager soundManager;

    private NavMeshAgent agent;

    public GameObject floatingTextPrefab;

    public LevelSpawner levelSpawner;

    bool stuck = false;

    // Start is called before the first frame update
    void Start()
    {
        baseStart();
    }

    protected void baseStart()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyData.speed;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = enemyData.maxHealth;
        currentDirection = enemyData.startingDirection;
        animator.SetFloat("speed", enemyData.speed);
        levelSpawner = GameObject.Find("Dungeon").GetComponent<LevelSpawner>();
        soundManager = GetComponent<EnemySoundManager>();
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

            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= enemyData.attackRange)
            {
                //if (attackCooldown >= enemyData.attackRefreshMin)
                //{
                    state = EnemyState.Attacking;
                //}
            }
            else if (EnemyManager.inEnemyRange.Contains(gameObject) || (state == EnemyState.Attacking || state == EnemyState.Following) 
            && (Vector3.Distance(player.transform.position, gameObject.transform.position) <= enemyData.chaseRange))
            {
                state = EnemyState.Following;
            }
            else
            {
                state = EnemyState.Patrolling;
            }
            //Debug.Log("Enemy health is " + health);
            prevPos = transform.position;
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
                    break;
                case EnemyState.Following:
                    agent.SetDestination(player.transform.position);
                    //position = FollowPlayer();
                    break;
                case EnemyState.Attacking:
                    position = Attack();
                    agent.ResetPath();
                    // code block
                    break;
                case EnemyState.Dead:
                    // this should do nothing
                    agent.ResetPath();
                    break;
                default:
                    // code block
                    break;
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
            soundManager.playDamagedSound();
        }
        Debug.Log("Enemy health updated - health is " + currentHealth);
        
        ShowFloatingText(amount);

        hitTimer = 0;
    }

    void ShowFloatingText(double amount){
        var floatingText  =  Resources.Load("Prefabs/Floating Text") as GameObject;
        GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = amount.ToString();
        text.GetComponent<TextMesh>().color = Color.yellow;
    }

    void Died()
    {
        Debug.Log(gameObject.name + " has died.");
        animator.SetTrigger("died");
        soundManager.playDeathSound();
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
            if (other.gameObject.tag == "Prop" || other.gameObject.tag == "Wall") {
                stuck = true;
            } else {
                stuck = false;
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
                    } else if (playerWeapon.weaponType == WeaponType.Staff)
                    {
                        damage = playerWeapon.weapons[3].GetComponent<WeaponManager>().Attack(this);
                    }
                }
                player.BroadcastMessage("OnAttack", PackageAttackInfo(gameObject, damage), SendMessageOptions.DontRequireReceiver);
                CommandInvoker.AddCommand(new DealDamageCommand(gameObject, damage));
            }
        }
    }

    void OnCollisionStay(Collision other) {
        if (other.gameObject.tag == "Prop" || other.gameObject.tag == "Wall") {
                stuck = true;
        } else {
                stuck = false;
        }
    }

    virtual protected Vector3 Patrol()
    {
        Vector3 position = rb.position;
        bool shouldRecalculate = (state != EnemyState.Dead && (agent.remainingDistance <= 0.2 || !agent.hasPath) && !agent.pathPending);
      
        if (shouldRecalculate || stuck)
        {
            Vector3 randomDestination = levelSpawner.destinations[UnityEngine.Random.Range(0, levelSpawner.destinations.Count)];
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDestination, out hit, 4, 1);
            position = hit.position;
            agent.SetDestination(position);
        }

        if (stuck)
            stuck = false;
        return position;
    }

    virtual protected Vector3 FollowPlayer()
    {
        Vector3 position = rb.position;

        if (player.GetComponent<Player>().isVisible())
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

                position += Time.deltaTime * enemyData.speed * seekDirection;
            }
        }
        return position;
    }


    virtual protected Vector3 Attack()
    {
        Vector3 position = rb.position;

        if (state != EnemyState.Dead && player.GetComponent<Player>().isVisible())
        {
            animator.SetTrigger("attacking");
            gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
            agent.ResetPath();
            soundManager.playAttackSound();
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
