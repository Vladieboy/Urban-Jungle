using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100;


    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        GetComponent<DeathHandler>().HandleDealth();
        print("SNAKE!!!!!");
    }
}
