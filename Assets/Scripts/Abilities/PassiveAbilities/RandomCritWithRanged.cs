using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCritWithRanged : MonoBehaviour
{
    private GameObject target;
    private int damage;
    [SerializeField] private int chanceToCrit;
    [SerializeField] private float critMultiplier;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("/Player/");
    }

    public void OnAttack(object[] variables)
    {
        target = (GameObject)variables[0];
        damage = (int)variables[1];

        if (target.CompareTag("Enemy") && player.GetComponent<PlayerCurrentWeapon>().weaponType == WeaponType.Bow)
        {
            if (Random.Range(1, 100) <= chanceToCrit)
            {
                CommandInvoker.AddCommand(new DealCriticalDamageCommand(target, damage, critMultiplier));
            }
        }
    }
}
