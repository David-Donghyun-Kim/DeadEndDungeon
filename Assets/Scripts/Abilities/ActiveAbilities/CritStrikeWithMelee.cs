using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CritStrikeWithMelee : MonoBehaviour
{
    [SerializeField] private float cooldown, critMultiplier;
    [SerializeField] private Sprite abilityImage;
    private bool isOnCooldown, critActive;

    private GameObject target, player, display;
    private int damage;

    private void Awake()
    {
        player = GameObject.Find("/Player/");
        isOnCooldown = false;
        critActive = false;
        display = GameObject.Find("/Player Unit Frame/Panel/AbilityCooldown");
        display.transform.GetChild(0).GetComponent<Image>().sprite = abilityImage;
    }
    public void OnAbilityActivate_0(GameObject target)
    {
        if (!isOnCooldown)
        {
            StartCoroutine(Cooldown());
            critActive = true;
        }
    }

    public void OnAttack(object[] variables)
    {
        target = (GameObject)variables[0];
        damage = (int)variables[1];

        if (critActive && target.CompareTag("Enemy") && player.GetComponent<PlayerCurrentWeapon>().weaponType == WeaponType.Sword)
        {
            CommandInvoker.AddCommand(new DealCriticalDamageCommand(target, damage, critMultiplier));
        }

        critActive = false;

        //StopCoroutine(Cooldown());
        //isOnCooldown = false;
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        display.GetComponent<DisplayClassAbility>().StartAbilityCooldown((int)cooldown);
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

}
