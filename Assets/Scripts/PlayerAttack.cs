using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    bool attackReady = true;
    //need to select an enemy

    private void Start()
    {
        GetComponent<EnemyHealth>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && attackReady)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        attackReady = false;

        //.DealDamage();

        yield return new WaitForSeconds(4);
        attackReady = true;
    }
}
