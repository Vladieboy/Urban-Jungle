using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int maxSpawn = 3;

    public GameObject enemy;
    public Transform enemyParentTransform;
    int xPos;
    int zPos;
    int enemyCount;

    List<Vector3> enemyPositions = new List<Vector3>();

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < maxSpawn)
        {
            Vector3 enemyPos = GetEnemySpawnPosition();

            foreach (var position in enemyPositions)
            {
                while (Vector3.Distance(position, enemyPos) <= 10)
                {
                    enemyPos = GetEnemySpawnPosition();
                }
            }

            enemyPositions.Add(enemyPos);

            GameObject newEnemy = Instantiate(enemy, enemyPos, Quaternion.identity);
            newEnemy.transform.parent = enemyParentTransform;
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
        
    }

    private Vector3 GetEnemySpawnPosition()
    {
        xPos = Random.Range(1, 29);
        zPos = Random.Range(1, 29);

        Vector3 enemyPos = new Vector3(xPos, 0f, zPos);
        return enemyPos;
    }
}
