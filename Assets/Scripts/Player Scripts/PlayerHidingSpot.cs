using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHidingSpot : MonoBehaviour
{

    
    Transform enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>().enemyParentTransform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < enemySpawner.childCount; i++)
        {

            enemySpawner.GetChild(i).GetComponent<EnemyAI>().IsPlayerHiding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < enemySpawner.childCount; i++)
        {

            enemySpawner.GetChild(i).GetComponent<EnemyAI>().IsPlayerHiding = false;
        }
    }
}
