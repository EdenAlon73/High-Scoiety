using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] bool HavePatrolPoints;
    [SerializeField] Transform pfFieldofView;
    [SerializeField] float fov = 90f;
    [SerializeField] float viewDistance = 2.5f;
    FieldOfView fieldOfView;
    
    public bool nextPatrolPoint=false;
    public float speed = 600f;
    private float originalSpeed;
    Vector2 originalPosition;
    public float nextWaypointDistance =3f;
    public bool seePlayer = false;
    private int _currentPoint = 0;
    
    private float originalRadios;
    private ConstantForce2D weedEffect;
    private Vector2 weedForce;
    Vector2 movment;
    Animator animator;
    public  bool isSmokeOut = false;
    bool lookBack = false;
    Vector3 aimDir;
    private Vector3 lastMoveDir;

    GameManager gameManager;


    Path path;
    int currentWaypoint = 0;
    bool reachedEndWayOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    [SerializeField] private EnemyStates currentState;
    public enum EnemyStates
    {
        
        Patrol=0,
        Follow=1,
        Sit=2,
    }

    private void DoAccordingToState()
    {
        switch (currentState)
        {
            case EnemyStates.Patrol:
                DoPatrol();
                break;

            

            case EnemyStates.Follow:
                DoFollow();
                break;

            case EnemyStates.Sit:
                DoSit();
                break;


           
                
        }
    }
    void CheckStateChange()
    {
        if (seePlayer) currentState = EnemyStates.Follow;

        else if (HavePatrolPoints&&!seePlayer) currentState = EnemyStates.Patrol;
        else if (!HavePatrolPoints&&!seePlayer) currentState = EnemyStates.Sit;

    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
      
        originalSpeed = speed;
        originalPosition = rb.position;
        weedEffect = GetComponent<ConstantForce2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);

        fieldOfView = Instantiate(pfFieldofView, null).GetComponent<FieldOfView>();
        fieldOfView.fov = fov;
        fieldOfView.viewDistance = viewDistance;
        
        
    }
    
    void UpdatePath()
    {
        
        
        CheckStateChange();
        DoAccordingToState();


    }
    void CheackForNextPatrolPoint()
    {
        if (nextPatrolPoint) _currentPoint++;
        
        
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    private void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndWayOfPath = true;
           
            return;
        }
        else
        {
            reachedEndWayOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);
        Vector2 force = direction * speed * Time.deltaTime;
        lastMoveDir = direction;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (HavePatrolPoints)
        {
            float distancePatrol = Vector2.Distance(rb.position, patrolPoints[_currentPoint].position);
        }
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        fieldOfView.setOrigin(transform.position);
        fieldOfView.SetAimDirection(GetAimDirection());
        


    }
    private void Update()
    {
        weedForce = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        FindTargetPlayer();
        EnemyAnimation();
        GetAimDirection();
        Debug.DrawLine(transform.position, transform.position + aimDir * 2f);

    }

    private void DoPatrol()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, patrolPoints[_currentPoint].position, OnPathComplete);
        float distance = Vector2.Distance(rb.position, patrolPoints[_currentPoint].position);
        if (distance < nextWaypointDistance)
        {
            if (!nextPatrolPoint)
                nextPatrolPoint = true;
            else nextPatrolPoint = false;
        }
        

        if (_currentPoint >= patrolPoints.Length-1 )
        {
            _currentPoint = 0;

        }
        CheackForNextPatrolPoint();
        speed = originalSpeed;
    }
    private void DoFollow()
    {
        Vector3 targetPosition = target.position;
        Vector3 dirToTarget = (targetPosition - transform.position);
        lastMoveDir = dirToTarget;
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        if(!isSmokeOut)
        speed = 150f + (originalSpeed * Time.deltaTime)*20f;

    }
    void DoSit()
    {
        float distance = Vector2.Distance(rb.position, originalPosition);
        if (distance > nextWaypointDistance)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, originalPosition, OnPathComplete);
        }
        
        

    }
   
   public IEnumerator SmokeOut()
    {
        speed = 0;
        yield return new WaitForSeconds(2);
        weedEffect.relativeForce = weedForce;
        weedEffect.torque = 1;
       
        speed = originalSpeed / 1.8f;
        yield return new WaitForSeconds(5f);
        
        isSmokeOut = false;
        weedEffect.relativeForce = new Vector2(0, 0);
        weedEffect.torque = 0;
        speed = originalSpeed;
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Weed")
        {
            isSmokeOut = true;
            StartCoroutine("SmokeOut");
        }
        if(other.tag=="Player")
        {
            gameManager.canPlayerMove = false;
            Invoke("CallLose", 5f);
        }
    }
    void EnemyAnimation()
    {
        movment.x = rb.velocity.x;
        movment.y = rb.velocity.y;
        animator.SetFloat("Horizontal", movment.x);
        animator.SetFloat("Vertical", movment.y);
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        animator.SetBool("High", isSmokeOut);
    }
  
    Vector3 GetAimDirection()
    {
        if (currentState == EnemyStates.Patrol)
        {
            aimDir = (patrolPoints[_currentPoint].position - transform.position).normalized;
        }
        else if (currentState == EnemyStates.Follow)
        {
            aimDir = (target.position - transform.position).normalized;
        }
        else aimDir = lastMoveDir;

        return aimDir;
    }
    
    void FindTargetPlayer()
    {
        if (Vector3.Distance(transform.position, target.position) < viewDistance)
        {
            
            // player inside view distance
            Vector3 dirToPlayer = (target.position - transform.position).normalized;
            if (Vector3.Angle(aimDir, dirToPlayer) > fov/2f  )
            {


                //player inside field of view
                RaycastHit2D rayCastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance);
                if (rayCastHit2D.collider != null)
                {
                    if (rayCastHit2D.collider.gameObject.tag == "Player")
                    {
                        seePlayer = true;

                    }

                }


            }
            else Invoke("ChangeSeePlayer", 4f);



        }
        
        

       
    }
    void ChangeSeePlayer()
    {
        if(isSmokeOut)
        seePlayer = false;
    }
    void CallLose()
    {
        gameManager.Lose();
    }
    
}


