using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainHealthCommand : ICommand
{
    private GameObject healTarget, display;
    private float healAmount;

    public GainHealthCommand(GameObject healTarget, float healAmount)
    {
        this.healTarget = healTarget;
        this.healAmount = healAmount;
        this.display = GameObject.Find("/Player Unit Frame/Panel/HealthBar/");
    }
    public void Execute()
    {
        if (healTarget.CompareTag("Enemy")) { healTarget.GetComponent<AbstractEnemyController>().ChangeHealth(healAmount); }
        else if (healTarget.CompareTag("Player")) { 
            if ((healAmount + healTarget.GetComponent<Player>().GetCurrentHealth()) <= healTarget.GetComponent<Player>().GetMaxHealth()) 
            {
                healTarget.GetComponent<Player>().AdjustHealth((int)healAmount);
                healTarget.GetComponent<Player>().ShowFloatingText(healAmount, "green", false);
                display.GetComponent<ChangePlayerHealthDisplay>().SetCurrentHealth(healTarget.GetComponent<Player>().GetCurrentHealth());
            } else {
                healAmount = healTarget.GetComponent<Player>().GetMaxHealth() - healTarget.GetComponent<Player>().GetCurrentHealth();
                healTarget.GetComponent<Player>().AdjustHealth((int)healAmount);
                healTarget.GetComponent<Player>().ShowFloatingText(healAmount, "green", false);
                display.GetComponent<ChangePlayerHealthDisplay>().SetCurrentHealth(healTarget.GetComponent<Player>().GetCurrentHealth());
            }
        }
    }
}
