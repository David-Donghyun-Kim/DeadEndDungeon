using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand
{
    private float jumpSpeed;
    private GameObject entity;
    public JumpCommand(GameObject entity, float jumpSpeed)
    {
        this.jumpSpeed = jumpSpeed;
        this.entity = entity;
    }
    public void Execute()
    {
        if (entity.CompareTag("Player")) 
        { 
            entity.GetComponent<PlayerController>().SetMovementDirectionY(jumpSpeed);
            entity.GetComponent<CharacterController>().Move(entity.GetComponent<PlayerController>().GetVelocity() * Time.deltaTime);
            Debug.Log("Player has jumped");
        }
        else
        {
            Debug.Log("JumpCommand: entity tag was" + entity.tag);
        }
    }
}
