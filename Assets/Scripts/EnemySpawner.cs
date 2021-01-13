using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform enemyParentTransform;
    public int xPos;
    public int zPos;
    public int enemyCount;


    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            xPos = Random.Range(1, 29);
            zPos = Random.Range(1, 29);

            GameObject newEnemy = Instantiate(enemy, new Vector3(xPos, 0f, zPos), Quaternion.identity);
            newEnemy.transform.parent = enemyParentTransform;
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }

}
