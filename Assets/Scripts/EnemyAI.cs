using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public enum EnemyStatus
    {
        Default,
        Alert,
        Provked
    }


    [SerializeField, Range(0f, 10f)] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    public bool IsPlayerHiding = false;

    NavMeshAgent navMeshAgent;


    ThirdPersonMovement character;

    public int EnemyAlertPhase;

    float distanceToTarget = Mathf.Infinity;
    public bool isProvoked = false;
    public bool isMovingBack = false;

    public bool IsProvoked() { return isProvoked; }

    // Start is called before the first frame update
    void Start()
    {
        EnemyAlertPhase = (int)EnemyStatus.Default;

        navMeshAgent = GetComponent<NavMeshAgent>();

        character = FindObjectOfType<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateChaseRange();


        if (isMovingBack)
        {
            var dist = Vector3.Distance(GetComponent<EnemyPatrol>().IntialPos(), transform.position);

            if (dist <= .2)
            {
                navMeshAgent.ResetPath();
                isMovingBack = false;
            }
        }

        distanceToTarget = Vector3.Distance(character.transform.position, transform.position);
        if (isProvoked)
        {
            //Attack
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
            isMovingBack = true;
        }
        else if (!isProvoked && isMovingBack)
        {
            //if enemy has lost player, return to starting positions

            navMeshAgent.SetDestination(GetComponent<EnemyPatrol>().IntialPos());


        }
    }

    public void UpdateChaseRange()
    {

        if (character.IsSneaking())
        {
            chaseRange = 3f;
        }
        else
        {
            chaseRange = 6f;
        }

        if (IsPlayerHiding) chaseRange -= 2f;
    }

    void FaceTarget()
    {

        Vector3 direction = (character.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

    }
    private void EngageTarget()
    {
        FaceTarget();

        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {

            ChaseTarget();

        }
        else if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        throw new NotImplementedException();
    }

    private void ChaseTarget()
    {

        navMeshAgent.SetDestination(character.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

    }
}
