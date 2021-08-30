using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class BecomeInvisible : MonoBehaviour
{
    [SerializeField] private float cooldown, duration;
    private bool cooldownOn;
    private GameObject target;

    private void Awake()
    {
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
        yield return new WaitForSeconds(duration);
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
