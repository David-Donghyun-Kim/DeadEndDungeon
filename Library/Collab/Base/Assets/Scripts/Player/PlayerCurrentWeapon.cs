using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType { None, Sword, Bow, Staff};

public class PlayerCurrentWeapon : MonoBehaviour
{

    //[System.NonSerialized]
    public WeaponType weaponType;
    public GameObject[] weapons;
    public PlayerController playerController;
    public GameObject arrow_Prefab;
    public Transform arrow_start_pos;

    Animator anim;




    private void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        if(weaponType == WeaponType.Sword)
        {
            // Sword weapon number = 0
            anim.SetBool("isSword", true);
            anim.SetBool("isBow", false);
            EnableWeapon(0);
        }
        if (weaponType == WeaponType.Bow)
        {
            // Bow weapon number = 1
            // Arrow weapon number = 2
            anim.SetBool("isBow", true);
            anim.SetBool("isSword", false);
            EnableWeapon(1, 2);
        }
    }

    void EnableWeapon(int num)
    {
        for(int i=0;i<weapons.Length;i++)
        {
            if (i == num)
            {
                weapons[i].SetActive(true);
                continue;
            }
            weapons[i].SetActive(false);
        }
    }
    void EnableWeapon(int num1, int num2)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == num1 || i == num2)
            {
                weapons[i].SetActive(true);
                continue;
            }
            weapons[i].SetActive(false);
        }
    }

    public float Attack()
    {
        Debug.Log("PlayerCurrentWeapon: Currently attacking.");
        float attack_cooltime = 0f;
        if (weaponType == WeaponType.Sword)
        {
            SwordAttack();
            attack_cooltime = 0.5f; // sword basic attack cool time
        }
        if (weaponType == WeaponType.Bow)
        {
            BowAttack();
            attack_cooltime = 1f;
        }
        return attack_cooltime;
    }

    void SwordAttack()
    {
        anim.SetTrigger("doAttack");
    }
    
    void BowAttack()
    {
        anim.SetTrigger("doAttack");
        // Invoke("ShootArrow", 0.5f);
    }

    void ShootArrow()
    {
        GameObject arrow = Instantiate(arrow_Prefab, arrow_start_pos.position, Quaternion.identity);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = Camera.main.transform.forward * 50;
    }
    void StaffAttack()
    {

    }

}
