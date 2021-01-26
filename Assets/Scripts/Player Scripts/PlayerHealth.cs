using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100;
    [SerializeField] public float maxHitPoints = 100;

    public SliderScript slider;

    public void Start()
    {
        
        slider.SetMaxValue(hitPoints);
    }
    public void TakeDamage(float damage)
    {
        
        slider.SetValue(hitPoints, hitPoints -= damage);
        if (hitPoints <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        GetComponent<DeathHandler>().HandleDealth();
        GetComponent<Animator>().SetBool("die", true);
        print("SNAKE!!!!!");
    }
}
