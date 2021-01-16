using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float enemyRange = 3f;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;
    private float characterVelocity = 2f;
     Vector3 movementDirection;
    Vector3 movementPerSecond;
    public Vector3 initialPos;
    public bool isWaiting = false;
    EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        //enemyAI = GetComponent<EnemyAI>();
        initialPos = transform.position;
        latestDirectionChangeTime = 0f;
        CalcuateNewMovementVector();
    }
    public Vector3 IntialPos()
    {
        return initialPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {

            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;
                CalcuateNewMovementVector();
            }
            
            //move enemy: 
            var newPos = new Vector3(transform.position.x + (movementPerSecond.x * Time.deltaTime),
            transform.position.y, transform.position.z + (movementPerSecond.z * Time.deltaTime));  
            //var newPos = new Vector3(transform.position.x + (movementPerSecond.x * Time.deltaTime),
            //transform.position.y + (movementPerSecond.y * Time.deltaTime), transform.position.z + (movementPerSecond.z * Time.deltaTime));

            var distance = Vector3.Distance(initialPos, newPos);
            
            if (distance > enemyRange)
            {
                    
         
                   
                StartCoroutine(Wait());
                
            }
            else
            {
                FaceTarget();
                transform.position = newPos;
            }
        }

    }

    void FaceTarget()
    {
        Quaternion lookRotation = Quaternion.LookRotation(movementDirection);   
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void CalcuateNewMovementVector()
    {
        
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy

        //movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0f, Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * characterVelocity;
    }

    IEnumerator Wait()
    {
        
      

        isWaiting = true;
        print("starting to wait");
        yield return new WaitForSeconds(3);
        print("done");
        isWaiting = false;
    }


}
