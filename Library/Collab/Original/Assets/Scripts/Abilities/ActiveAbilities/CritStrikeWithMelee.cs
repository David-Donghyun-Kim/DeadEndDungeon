using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritStrikeWithMelee : MonoBehaviour
{
    [SerializeField] private float cooldown, critMultiplier;
    private bool isOnCooldown;

    private GameObject target, player;
    private int damage;

    private void Awake()
    {
        player = GameObject.Find("/Player/");
        isOnCooldown = false;
    }
    public void OnAbilityActivate_0(GameObject target)
    {
        if (!isOnCooldown)
        {
            StartCoroutine(Cooldown());
        }
    }

    public void OnAttack(object[] variables)
    {
        target = (GameObject)variables[0];
        damage = (int)variables[1];

        if (target.CompareTag("Enemy") && player.GetComponent<PlayerCurrentWeapon>().weaponType == WeaponType.Sword)
        {
            CommandInvoker.AddCommand(new DealCriticalDamageCommand(target, damage, critMultiplier));
        }

        StopCoroutine(Cooldown());
        isOnCooldown = false;
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

}
