using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBase : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle = 0,
        Patrol = 1,
        Follow = 2,
        Attack = 3,
        Confused = 4
    }

    [SerializeField] private NavMeshAgent agent;

    private bool isStateChanged = false;
    private EnemyStates prevState;
    [SerializeField] private EnemyStates currentState;
    //[SerializeField] private EnemyStates defaultState;
    [SerializeField] private bool idleIsDefault;

    [SerializeField] private Transform player;

    [SerializeField] private float minDistToFollow;
    [SerializeField] private float minDistToAttack;

    [SerializeField] private float speed;

    [SerializeField] private Vector3 randomNewPlace;

    [SerializeField] private Transform[] patrolPoints;
    private int _currentPoint = 0;

    [SerializeField] private List<Vector3> patrolPositions;
    
    private void Awake()
    {
        //currentState = defaultState;
        foreach (var point in patrolPoints)
        {
            patrolPositions.Add(point.position);
        }
        
        agent.speed = speed;
        SetStateToDefault();

        if(currentState == EnemyStates.Patrol)
        {
            isStateChanged = true;
            DoConfuse();
        }
        /*if (idleIsDefault)
        {
            currentState = EnemyStates.Idle;
        }
        else
        {
            currentState = EnemyStates.Patrol;
        }*/
    }

    private void Update()
    {
        CheckStateChange();
        DoAccordingToState();
    }

    private void CheckStateChange()
    {
        prevState = currentState;

        var distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= minDistToAttack)
        {
            currentState = EnemyStates.Attack;
        }
        else if(distToPlayer <= minDistToFollow)
        {
            currentState = EnemyStates.Follow;
        }
        else
        {
            SetStateToDefault();
        }

        isStateChanged = prevState != currentState;
    }

    private void DoAccordingToState()
    {
        switch (currentState)
        {
            case EnemyStates.Patrol:
                DoPatrol();
                break;
            
            case EnemyStates.Confused:
                DoConfuse();
                break;

            case EnemyStates.Follow:
                DoFollow();
                break;

            case EnemyStates.Attack:
                DoAttack();
                break;

            case EnemyStates.Idle:
            default:
                agent.isStopped = true;
                break;
        }
    }

    private void SetStateToDefault()
    {
        currentState = idleIsDefault ? EnemyStates.Idle : EnemyStates.Patrol;
    }

    private void DoConfuse()
    {
        if(isStateChanged || (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0))
        {
            randomNewPlace = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(10f, 11f);
            agent.SetDestination(randomNewPlace);
        }
    }
    
    private void DoPatrol()
    {
        if(isStateChanged || (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0))
        {
            _currentPoint++;
            if (patrolPositions.Count <= _currentPoint)
            {
                _currentPoint = 0;
            }

            var newDest = patrolPositions[_currentPoint];
            agent.SetDestination(newDest);
        }
    }

    private void DoFollow()
    {
        agent.isStopped = false;
        //transform.position += (player.position - transform.position).normalized * speed * Time.deltaTime;
        agent.SetDestination(player.position);
    }

    private void DoRunAway()
    {
        transform.position += (transform.position - player.position).normalized * speed * Time.deltaTime;
    }

    private void DoAttack()
    {
        Destroy(player.gameObject);
        Destroy(gameObject);
    }
}
