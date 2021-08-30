using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastFireball : MonoBehaviour
{
    private GameObject caster, display;
    private Transform spawn;
    [SerializeField] private GameObject fireball;
    [SerializeField] private Sprite abilityImage;

    [SerializeField] private float cooldown;
    private bool isOnCooldown;
    private void Awake()
    {
        isOnCooldown = false;
        display = GameObject.Find("/Player Unit Frame/Panel/AbilityCooldown");
        display.transform.GetChild(0).GetComponent<Image>().sprite = abilityImage;
    }

    IEnumerator CooldownActivate()
    {
        isOnCooldown = true;
        display.GetComponent<DisplayClassAbility>().StartAbilityCooldown((int)cooldown);
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }


    public void OnAbilityActivate_0(GameObject caster)
    {
        if (!isOnCooldown)
        {
            this.caster = caster;
            this.spawn = caster.transform;
            spawn.SetPositionAndRotation(new Vector3(spawn.position.x, spawn.position.y - 4, spawn.position.z), spawn.rotation);

            GameObject.Instantiate(fireball, spawn, false);

            StartCoroutine(CooldownActivate());
        }

        
    }
}
