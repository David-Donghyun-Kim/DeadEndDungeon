using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReduceIncomingDamage : MonoBehaviour
{
    [SerializeField] private float damageReduction, duration, cooldown;
    [SerializeField] private Sprite abilityImage;
    private GameObject player, display;
    private bool isOnCooldown;

    private void Awake()
    {
        this.player = GameObject.Find("/Player/");
        this.display = GameObject.Find("/Player Unit Frame/Panel/AbilityCooldown");
        display.transform.GetChild(0).GetComponent<Image>().sprite = abilityImage;
        isOnCooldown = false;
    }

    IEnumerator AbilityActivate()
    {
        if (damageReduction > 1.0f)
        {
            Debug.Log("Warning: Damage Reduction was set to a value greater than 1.0; Ignoring ability");
            yield return new WaitForSeconds(0.5f);
            StopCoroutine("CooldownActive");
            if (isOnCooldown) { isOnCooldown = false; }
        }
        else
        {
            player.GetComponent<Player>().SetDmgRed(1 - damageReduction);
            display.GetComponent<DisplayClassAbility>().StartAbilityCooldown((int)cooldown);
            yield return new WaitForSeconds(duration);
            player.GetComponent<Player>().SetDmgRed(1.0f);
        }
        
    }

    IEnumerator CooldownActive()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

    private void OnAbilityActivate_0(GameObject player)
    {
        if (!isOnCooldown)
        {
            StartCoroutine("AbilityActivate");
            StartCoroutine("CooldownActive");
        }
    }
}
