using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float restPeriod = 4f;
    [SerializeField] float damage = 20f;

    bool attackReady = true;
    EnemyHealth currentTarget;
    //need to select an enemy

    private void Start()
    {
        GetComponent<EnemyHealth>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && attackReady && !currentTarget.Equals(null))
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        attackReady = false;
        
        //deplete stamina on attack

        currentTarget.DealDamage(damage);

        yield return new WaitForSeconds(restPeriod);
        attackReady = true;
    }


    /**
     *Fix later
     *can't compare in single statement for some reason
     */
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            if (currentTarget != null)
            {
                if (currentTarget.name != collision.name) currentTarget = collision.gameObject.GetComponent<EnemyHealth>();
            }
            else if (currentTarget == null)
            {
                currentTarget = collision.gameObject.GetComponent<EnemyHealth>();
            }

        }

    }


}
