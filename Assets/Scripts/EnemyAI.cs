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
        Reseting,
        Provoked,
        Attacking
    }

    public EnemyStatus EnemyAlertPhase;

    public float timeUntilGiveUp = 2f;

    [SerializeField, Range(60f, 120f)] float enemyFOV = 60f;
    [SerializeField, Range(0f, 10f)] float aggroRange = 5f;
    [SerializeField] float turnSpeed = 5f;

    public bool IsPlayerHiding = false;

    NavMeshAgent navMeshAgent;

    Timer timer;

    ThirdPersonMovement character;


    float distanceToTarget = Mathf.Infinity;
    public bool isMovingBack = false;




    // Start is called before the first frame update
    void Start()
    {
        EnemyAlertPhase = (int)EnemyStatus.Default;
        timer = GetComponent<Timer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        character = FindObjectOfType<ThirdPersonMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(character.transform.position, transform.position);
        //changes the vision range of the enenmy based on player actions
        //NOTES: Add player hidden modifier
        UpdateChaseRange();

        bool _isPlayerSighted = PlayerSighted();

        if (!_isPlayerSighted && EnemyAlertPhase == EnemyStatus.Reseting)
        {
            var dist = Vector3.Distance(GetComponent<EnemyPatrol>().IntialPos(), transform.position);
            print(dist);
            if (dist <= 2)
            {
                navMeshAgent.ResetPath();
                timer.ResetTimer();
                EnemyAlertPhase = EnemyStatus.Default;
                
            }
            else navMeshAgent.SetDestination(GetComponent<EnemyPatrol>().IntialPos());

        }

        if (!_isPlayerSighted && EnemyAlertPhase == EnemyStatus.Provoked && timer.timeRemaining == 0)
        {
            EnemyAlertPhase = EnemyStatus.Alert;
            timer.StartTimer();
        }

        if (!_isPlayerSighted && EnemyAlertPhase == EnemyStatus.Alert && timer.timeRemaining == 0)
        {
            print("give up");


            EnemyAlertPhase = EnemyStatus.Reseting;
            navMeshAgent.SetDestination(GetComponent<EnemyPatrol>().IntialPos());
        }

        if (_isPlayerSighted && EnemyAlertPhase == EnemyStatus.Default)
        {
            EnemyAlertPhase = EnemyStatus.Provoked;
        }
        else if (_isPlayerSighted && (EnemyAlertPhase == EnemyStatus.Provoked || EnemyAlertPhase == EnemyStatus.Attacking))
        {
            timer.ResetTimer();
            EngageTarget();
        }
        else if (!_isPlayerSighted && (EnemyAlertPhase == EnemyStatus.Provoked || EnemyAlertPhase == EnemyStatus.Attacking))
        {

            EngageTarget();
            timer.timerIsRunning = true;
        } else if (!_isPlayerSighted && EnemyAlertPhase == EnemyStatus.Default)
        {
            timer.StopTimer();
        }

    }


    private bool PlayerSighted()
    {
        bool _isSighted = false;
        Vector3 direction = (character.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < enemyFOV * 0.5f)
        {

            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, aggroRange))
            {

                if (character.CompareTag(hit.collider.gameObject.tag))
                {


                    _isSighted = true;
                }

            }

        }

        return _isSighted;
    }

    public void UpdateChaseRange()
    {

        switch (EnemyAlertPhase)
        {
            case EnemyStatus.Default:
                aggroRange = 5f;
                break;
            case EnemyStatus.Alert:
                aggroRange = 7f;
                break;
            case EnemyStatus.Provoked:
                aggroRange = 10f;
                break;
            default:
                break;
        }


        if (character.IsSneaking())
        {
            aggroRange = 3f;
        }


        if (IsPlayerHiding) aggroRange -= 2f;
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
            timer.StopTimer();
            
            ChaseTarget();

        }
        else if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            EnemyAlertPhase = EnemyStatus.Attacking;
            if (timer.timeRemaining == 0)
            {
                AttackTarget();
                timer.ResetTimer();
                timer.StartTimer();
            } else
            {

                timer.StartTimer();
            }


        }
    }

    private void AttackTarget()
    {
        character.GetComponent<PlayerHealth>().TakeDamage(20);
    }

    private void ChaseTarget()
    {
        if (EnemyAlertPhase == EnemyStatus.Attacking)
        {
            EnemyAlertPhase = EnemyStatus.Provoked;
            timer.StartTimer();
        }
        navMeshAgent.SetDestination(character.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        DrawFOV();

    }

    private void DrawFOV()
    {
        float halfFOV = enemyFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRayDirection * aggroRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * aggroRange);
    }
}
