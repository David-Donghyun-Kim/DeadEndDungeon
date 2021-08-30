using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatDeadEnemies : MonoBehaviour
{
    [SerializeField] private float healAmount;
    private float cooldown = 1f;
    private bool cooldownOn;

    private void Start()
    {
        cooldownOn = false;
    }

    public void OnAbilityActivate_0(GameObject target)
    {
        if (!cooldownOn)
        {
            StartCoroutine(CooldownActivate());
            if (target.CompareTag("Enemy") && target.GetComponent<AbstractEnemyController>().state == AbstractEnemyController.EnemyState.Dead)
            {
                CommandInvoker.AddCommand(new DespawnEnemyCorpseCommand(target));
                CommandInvoker.AddCommand(new GainHealthCommand(GameObject.Find("/Player/"), healAmount));
                Debug.Log("Enemy Corpse Consumed");
            }
        }
    }

    IEnumerator CooldownActivate()
    {
        cooldownOn = true;
        yield return new WaitForSeconds(cooldown);
        cooldownOn = false;
    }
}
