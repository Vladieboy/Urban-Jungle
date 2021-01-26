using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    // Start is called before the first frame update
    public void DealDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        Destroy(gameObject);
    }
}
