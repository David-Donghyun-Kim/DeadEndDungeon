using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Enemy", menuName = "TestEnemy")]
public class SO_Enemy :ScriptableObject
{
    public float sightRange;
    public float attackRange;
    public float chaseRange;
    public float attackRefreshMin;

    public int baseAttackDamage;

    public float speed;
    public float startingDirection;
    public bool xAxis;

    public double maxHealth;
}
