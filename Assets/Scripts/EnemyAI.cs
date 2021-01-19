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
    Animator anim;


    float distanceToTarget = Mathf.Infinity;
    public bool isMovingBack = false;




    // Start is called before the first frame update
    void Start()
    {
        EnemyAlertPhase = (int)EnemyStatus.Default;
        timer = GetComponent<Timer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

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
        bool _shouldEngage = ShouldEngageTarget();

        if (_isPlayerSighted) FaceTarget();

        switch (EnemyAlertPhase)
        {
            case EnemyStatus.Default:
                if (!_isPlayerSighted) timer.StopTimer();
                else EnemyAlertPhase = EnemyStatus.Provoked;
                break;
            case EnemyStatus.Alert:

                if (_isPlayerSighted)
                {

                }
                else if (_isPlayerSighted && timer.timeRemaining == 0)
                {
                    EnemyAlertPhase = EnemyStatus.Reseting;
                    navMeshAgent.SetDestination(GetComponent<EnemyPatrol>().IntialPos());
                }

                break;
            case EnemyStatus.Reseting:
                if (_isPlayerSighted)
                {
                    EnemyAlertPhase = EnemyStatus.Provoked;
                }
                else
                {

                    var dist = Vector3.Distance(GetComponent<EnemyPatrol>().IntialPos(), transform.position);

                    if (dist <= 2)
                    {
                        navMeshAgent.ResetPath();
                        timer.ResetTimer();
                        EnemyAlertPhase = EnemyStatus.Default;

                    }
                    else navMeshAgent.SetDestination(GetComponent<EnemyPatrol>().IntialPos());
                }
                break;
            case EnemyStatus.Provoked:
                if (_isPlayerSighted) timer.ResetTimer();
                else if (!_isPlayerSighted && timer.timeRemaining == 0)
                {
                    EnemyAlertPhase = EnemyStatus.Alert;
                    timer.StartTimer();
                }
                else if (!_isPlayerSighted) timer.timerIsRunning = true;
                EngageTarget();
                break;
            case EnemyStatus.Attacking:

                //if player is in range, attack
                //else set status to provoked
                if (_isPlayerSighted) AttackTarget();
                else if (!_isPlayerSighted) EnemyAlertPhase = EnemyStatus.Provoked;
                break;
            default:
                break;
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


            //increase range on aggro
            if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, aggroRange))
            {



                //if (character.CompareTag(hit.collider.gameObject.tag))
                if (character.tag == "Player")
                {
                    anim.SetTrigger("bark");

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

            //if (timer.timeRemaining == 0)
            //{
            //    AttackTarget();
            //    timer.ResetTimer();
            //    timer.StartTimer();
            //}
            //else
            //{

            //    timer.StartTimer();
            //}


        }
    }
    private bool ShouldEngageTarget()
    {
        distanceToTarget = Vector3.Distance(character.transform.position, transform.position);
        var _shouldEngage = false;
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            _shouldEngage = true;


        }
        return _shouldEngage;
    }

    public void AttackTarget()
    {
        //anim.SetBool("attack", true);
        character.GetComponent<PlayerHealth>().TakeDamage(20);
    }

    private void ChaseTarget()
    {
        anim.ResetTrigger("bark");
        anim.SetBool("run", true);
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
