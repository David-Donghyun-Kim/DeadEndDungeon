using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    public AudioSource attackSound;
    public AudioSource damagedSound;
    public AudioSource deathSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void playAttackSound()
    {
        attackSound.Play();
    }
    public void playDamagedSound()
    {
        damagedSound.Play();
    }
    public void playDeathSound()
    {
        deathSound.Play();
    }
}
