using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Player player;
    //public GameObject enemy;
    // public Attribute[] enemyAttributes;
    public WeaponType weapon;
    public Collider weaponCollider;

    public bool playerIsAttacking;

    // Start is called before the first frame update
    void Start()
    {
        weapon = player.currentWeapon.weaponType;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        playerIsAttacking = player.GetComponent<PlayerController>().GetAttackingState();
        weapon = player.currentWeapon.weaponType;

        if (playerIsAttacking) {
            gameObject.GetComponent<Collider>().enabled = true;
            //Debug.Log("weapon collider enabled");
        } else { 
            gameObject.GetComponent<Collider>().enabled = false;
            //Debug.Log("weapon collider disabled");
        }
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("1 weapon trigger entered and weapon type is " + weapon);

    //    if (other.gameObject.tag == "Player" && weapon != WeaponType.Bow)
    //    {
    //        return;
    //    }

    //    Debug.Log("Weapon " + weapon + " hit " + other.gameObject.name);

    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("Hit an enemy");
    //        int damage = 0;
            
    //        if (weapon == WeaponType.Sword)
    //            damage = CalculateSwordDamage();
    //        else if (weapon == WeaponType.Bow)
    //            damage = CalculateBowDamage();
    //        else if (weapon == WeaponType.Staff)
    //            damage = CalculateStaffDamage();

    //        player.BroadcastMessage("OnAttack", PackageAttackInfo(other.gameObject, damage), SendMessageOptions.DontRequireReceiver);
    //        //other.gameObject.GetComponent<AbstractEnemyController>().ChangeHealth(-damage);
    //        CommandInvoker.AddCommand(new DealDamageCommand(other.gameObject, damage));
    //    }
    //}

    public int Attack(AbstractEnemyController enemy)
    {
        weapon = player.currentWeapon.weaponType;
        Debug.Log("Weapon: " + gameObject.name + ": " + weapon + " is attacking");
        int damage = 0;
        if (weapon == WeaponType.Sword)
            damage = CalculateSwordDamage();
        else if (weapon == WeaponType.Bow)
            damage = CalculateBowDamage();
        else if (weapon == WeaponType.Staff)
            damage = CalculateStaffDamage();

        return -damage;

        //player.BroadcastMessage("OnAttack", PackageAttackInfo(enemy.gameObject, damage), SendMessageOptions.DontRequireReceiver);
        //other.gameObject.GetComponent<AbstractEnemyController>().ChangeHealth(-damage);
        //CommandInvoker.AddCommand(new DealDamageCommand(enemy.gameObject, damage));
    }

public int CalculateSwordDamage() 
    {
        int str = player.GetAttributeAsInt(BuffType.Strength);
        int baseDmg = player.GetAttributeAsInt(BuffType.BaseDamage);

        return ((str / 2) + baseDmg);
    }

    public int CalculateBowDamage()
    {
        int dex = player.GetAttributeAsInt(BuffType.Dexterity);
        int baseDmg = player.GetAttributeAsInt(BuffType.BaseDamage);

        return ((dex/ 2) + baseDmg);
    }

    public int CalculateStaffDamage()
    {
        int intellect = player.GetAttributeAsInt(BuffType.Intellect);
        int baseDmg = player.GetAttributeAsInt(BuffType.BaseDamage);

        // add spell modifiers

        return ((intellect/ 2) + baseDmg);
    }
}
