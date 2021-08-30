using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class BecomeInvisible : MonoBehaviour
{
    [SerializeField] private float cooldown, duration;
    [SerializeField] private Sprite abilityImage;
    private bool cooldownOn;
    private GameObject target, player, display;

    private void Awake()
    {
        player = GameObject.Find("/Player/");
        this.display = GameObject.Find("/Player Unit Frame/Panel/AbilityCooldown");
        display.transform.GetChild(0).GetComponent<Image>().sprite = abilityImage;
        cooldownOn = false;
    }
    IEnumerator CooldownActivate()
    {
        cooldownOn = true;
        yield return new WaitForSeconds(cooldown);
        cooldownOn = false;
    }

    IEnumerator AbilityActive()
    {
        this.GetComponent<SkinnedMeshRenderer>().enabled = false;
        player.GetComponent<Player>().setVisibility(false);
        display.GetComponent<DisplayClassAbility>().StartAbilityCooldown((int)cooldown);
        yield return new WaitForSeconds(duration);
        player.GetComponent<Player>().setVisibility(true);
        this.GetComponent<SkinnedMeshRenderer>().enabled = true;

    }

    private void OnAbilityActivate_0(GameObject target)
    {
        
        if (!cooldownOn)
        {
            StartCoroutine(AbilityActive());
            StartCoroutine(CooldownActivate());
        }
    }
}
