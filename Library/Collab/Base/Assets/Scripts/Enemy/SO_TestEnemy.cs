using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Enemy", menuName = "TestEnemy")]
public class SO_TestEnemy :ScriptableObject
{
    public float attackRange;
    public float attackRefreshMin;

    public float speed;
    public float direction;
    public bool xAxis;

    public double maxHealth;
}
