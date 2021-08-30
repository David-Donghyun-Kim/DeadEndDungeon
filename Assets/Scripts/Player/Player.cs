using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject baseAttributeInventory;
    public PlayerCurrentWeapon currentWeapon;
    public Attributes playerAttributes;
    public Camera cameraCast;
    public StatusCondition condition;
    public bool dungeonEntered = false;
    private float rayDistance;
    private float distance = 13;
    private int health = 20;
    private int maxHealth = 20;
    private float inateDmgReduct = 1.0f;

    private bool visibility;
	
	public void Start()
	{	
        condition = StatusCondition.None;
        Invoke("WeaponSet", 0.2f);

        visibility = true;
	}

    public void WeaponSet() {
        if (inventory.FindWeaponOnInventory() == null) inventory.SetStartingWeapon();
        Debug.Log("weapon is " + inventory.equippedWeapon);
        currentWeapon.weaponType = inventory.equippedWeapon;
        currentWeapon.attack_type = inventory.equippedWeaponAttackType;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            inventory.Save();
            baseAttributeInventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inventory.Load();
            baseAttributeInventory.Load();
        }
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     UseHealthPot();
        // }
    }

    public int GetCurrentHealth() {
        // use stamina however you want to calculate health
        //int stam = GetAttributeAsInt(BuffType.Stamina);

        // if you want to use some sort of base value for health,
        // this might be useful (set through the class attribute item objects)
        // otherwise, just comment it out
        //int baseHealth = GetAttributeAsInt(BuffType.Health);

        return health; 
    }

    public int GetMaxHealth() {
        return maxHealth; 
    }

    public void AdjustHealth(int amount) {
        health += amount;
        if (health < 0) health = 0;
    }

    public float GetDR() { return inateDmgReduct; }
    public void SetDmgRed(float newVal)
    {
        inateDmgReduct = newVal;
    }

    public void RefillHealthToFull(int amount) {
        health = maxHealth;
    }

    public void ConsumeHealthPotion() {
        InventorySlot potion = inventory.FindItemOnInventory(6);
        if (potion.item != null) {
            int healAmount = potion.item.buffs[0].value;
            CommandInvoker.AddCommand(new GainHealthCommand(gameObject, healAmount));
            if (potion.amount > 1) {
                potion.amount = potion.amount - 1;
                potion.UpdateSlot(potion.item, potion.amount);
            } else { 
                potion.RemoveItem(); 
            }
        }
        Debug.Log("called");
    }

    public void PickUp () {
        int layerMask = 1 << LayerMask.NameToLayer("Loot");
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = cameraCast.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, distance, layerMask))
        {
            if (raycastHit.collider.gameObject.tag == "Item") {
                ObtainItem(raycastHit);
            } else if (raycastHit.collider.gameObject.tag == "Weapon") {
                ObtainWeapon(raycastHit);
            } 
        }
    }
    
     public void ObtainItem(RaycastHit raycastHit)
    {
        var groundItem = raycastHit.collider.gameObject.GetComponent<GroundItem>();
        Item _item = new Item(groundItem.item);
        if (inventory.AddItem(_item))
            Destroy(raycastHit.collider.gameObject);
    }
    
    public void ObtainWeapon(RaycastHit raycastHit)
    {
        var groundItem = raycastHit.collider.GetComponent<GroundItem>();

        if (currentWeapon.weaponType != WeaponType.None) // if  a weapon is equipped
            inventory.SwitchWeapon(raycastHit.collider);

        currentWeapon.weaponType = WeaponType.None;
        currentWeapon.attack_type = WeaponAttackType.None;
        
        Item _item = new Item(groundItem.item);
        if (inventory.AddItem(_item))
        {
            currentWeapon.weaponType = inventory.equippedWeapon;
            currentWeapon.attack_type = inventory.equippedWeaponAttackType;
            Debug.Log(currentWeapon.attack_type);
            Destroy(raycastHit.collider.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        baseAttributeInventory.Clear();
    }

    public int GetAttributeAsInt(BuffType att)
    {       
        for (int i = 0; i < playerAttributes.attributes.Length; i++)
            if (playerAttributes.attributes[i].type == att)
                return playerAttributes.attributes[i].value.ModifiedValue;

        return 0;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            //TODO update the damage amount
            Debug.Log("Player taking damage from " + other.gameObject.tag);
            EnemyProjectile projectile = other.gameObject.GetComponent<EnemyProjectile>();
            AbstractEnemyController enemy = other.gameObject.GetComponentInParent<AbstractEnemyController>();
            int damage = 0;
            if (projectile != null)
            {
                damage = projectile.damage;
            }
            else if (enemy != null)
            {
                damage = enemy.enemyData.baseAttackDamage;
                enemy.gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
            }
            else
            {
                damage = 5;
            }
            

            CommandInvoker.AddCommand(new DealDamageToPlayerCommand(damage));

            if (inateDmgReduct == 1.0f)
                ShowFloatingText(damage, "red", false);
            else 
                ShowFloatingText((damage/2), "red", true);
        }
    }

    public void ShowFloatingText(double amount, string color, bool damageReduced){
        var floatingText  =  Resources.Load("Prefabs/Floating Text") as GameObject;
        GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        if (color == "red")
        {
            if (damageReduced)
                text.GetComponent<TextMesh>().text = "-" + amount.ToString() + "\nTaking Reduced Damage";
            else 
                text.GetComponent<TextMesh>().text = "-" + amount.ToString();
            text.GetComponent<TextMesh>().color = Color.red;
        } else {
            text.GetComponent<TextMesh>().text = "+" + amount.ToString();
            text.GetComponent<TextMesh>().color = Color.green;
        }
    }

    public void setVisibility(bool setter)
    {
        visibility = setter;
    }

    public bool isVisible()
    {
        return visibility;
    }
}
