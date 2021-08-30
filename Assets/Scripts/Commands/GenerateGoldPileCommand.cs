using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateGoldPileCommand : ICommand
{
    private Transform spawn;
    private GameObject _goldPrefab, goldPile;
    public GenerateGoldPileCommand(Transform spawn)
    {
        this.spawn = spawn;
        this._goldPrefab = Resources.Load("Prefabs/Items/Gold.prefab") as GameObject;
    }
    public void Execute()
    {
        goldPile = GameObject.Instantiate(_goldPrefab, spawn, false);
    }
}
