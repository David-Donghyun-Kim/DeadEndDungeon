using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCollisionEffect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particles;
    [SerializeField]
    ParticleSystem fog;
    [SerializeField]
    GameObject lights;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌!!");
        particles.Play();
        fog.Play();
        lights.SetActive(true);
    }
}
