using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollisionManager : MonoBehaviour
{
    [SerializeField] private float damage, timer;

    private void Awake()
    {
        timer = 6.0f;
        StartCoroutine("LifetimeToDestroy");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.ToString() == "Default") 
        {
            CommandInvoker.AddCommand(new DamageGOsInSphereAreaCommand(this.gameObject.GetComponentInChildren<SphereCollider>(), damage));
        }
    }

    IEnumerable LifetimeToDestroy()
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
