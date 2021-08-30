using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelOnTrigger : MonoBehaviour
{

    [SerializeField] private GameObject dungeonGeneratorGO;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player passed through portal");
            GameObject.Find("/Dungeon/").GetComponent<LevelSpawner>().StartCoroutine("GoToNextLevel");
        }

    }

}
