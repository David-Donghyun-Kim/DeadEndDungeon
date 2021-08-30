using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToPlayerCommand : ICommand
{
    private int damage;
    private GameObject player, display;
    public DealDamageToPlayerCommand(int damage)
    {
        this.damage = damage;
        this.player = GameObject.Find("Player");
        this.display = GameObject.Find("/Player Unit Frame/Panel/HealthBar/");
    }
    public void Execute() 
    {
        player.GetComponent<Player>().AdjustHealth((int) (-damage * player.GetComponent<Player>().GetDR()));
        display.GetComponent<ChangePlayerHealthDisplay>().SetCurrentHealth(player.GetComponent<Player>().GetCurrentHealth());

        if (player.GetComponent<Player>().GetCurrentHealth() <= 0)
        {
            player.GetComponent<PlayerController>().Die();
        }
    }
}
