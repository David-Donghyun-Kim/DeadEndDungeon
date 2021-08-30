using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType { None, Sword, Bow, Staff};

public class PlayerCurrentWeapon : MonoBehaviour
{

    //[System.NonSerialized]
    public WeaponType weaponType;
    public WeaponAttackType attack_type;
    public GameObject[] weapons;
    public PlayerController playerController;
    public GameObject arrow_Prefab;
    public Transform arrow_start_pos;
    public AudioClip[] sword_sounds;

    private int comboStep;
    private bool comboPossible;
    private bool sword_pickup = false;
    AudioSource soundSource;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        soundSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(weaponType == WeaponType.Sword)
        {
            // Sword weapon number = 0
            anim.SetBool("isSword", true);
            anim.SetBool("isBow", false);
            anim.SetBool("isStaff", false);
            EnableWeapon(0);
            if(!sword_pickup)
            {
                sword_pickup = true;
                soundSource.clip = sword_sounds[0];
                soundSource.Play();
            }
        }
        if (weaponType == WeaponType.Bow)
        {
            // Bow weapon number = 1
            // Arrow weapon number = 2
            anim.SetBool("isBow", true);
            anim.SetBool("isSword", false);
            anim.SetBool("isStaff", false);
            EnableWeapon(1, 2);

            sword_pickup = false;
        }

        if(weaponType == WeaponType.Staff)
        {
            // Staff weapon number = 3
            anim.SetBool("isStaff", true);
            anim.SetBool("isSword", false);
            anim.SetBool("isBow", false);
            EnableWeapon(3);

            sword_pickup = false;
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
        Debug.Log("Weapon is type " + attack_type);
        float attack_cooltime = 0f;
        if (weaponType == WeaponType.Sword)
        {
            SwordAttack();
            attack_cooltime = 0f; // sword basic attack cool time
        }
        if (weaponType == WeaponType.Bow)
        {
            BowAttack();
            attack_cooltime = 1f;
        }
        if (weaponType == WeaponType.Staff)
        {
            StaffAttack();
            attack_cooltime = 1f;
        }

        return attack_cooltime;
    }

    void SwordAttack()
    {
        anim.SetTrigger("doSwordAttack");
        if(comboStep == 0)
        {
            anim.SetTrigger("doSwordAttack");
            soundSource.clip = sword_sounds[1];
            soundSource.Play();
            comboStep = 1;
            return;
        }
        if(comboStep != 0)
        {
            if(comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
    }
    public void ComboPossible()
    {
        comboPossible = true;
    }
    public void Combo()
    {
        if (comboStep == 2)
        {
            anim.Play("Sword-Attack-R3_Modi");
            soundSource.clip = sword_sounds[1];
            soundSource.Play();
            comboStep += 1;
        }

        if (comboStep == 4)
        {
            anim.Play("Sword-Attack-R4_Modi");
            soundSource.clip = sword_sounds[2];
            soundSource.Play();
        }

    }
    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
    }

    void BowAttack()
    {
        anim.SetTrigger("doBowAttack");
    }

    void StaffAttack()
    {
        anim.SetTrigger("doStaffAttack");
    }

}
