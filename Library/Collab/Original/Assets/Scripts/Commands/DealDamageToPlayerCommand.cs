using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToPlayerCommand : ICommand
{
    private int damage;
    private GameObject player;
    public DealDamageToPlayerCommand(int damage)
    {
        this.damage = damage;
        this.player = GameObject.Find("/Player/");
    }
    public void Execute() 
    {
        player.GetComponent<Player>().health -= damage;
    }
}
