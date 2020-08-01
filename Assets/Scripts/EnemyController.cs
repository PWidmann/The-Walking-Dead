using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private enum State { idle, pathing, chaseTarget };
    private State state;
    
    public float chaseRadius = 10f;
    public float idleTime = 3f;
    private float timer = 0f;

    private float wayPointSearchRadius = 15f;
    private int currentWayPoint = 0;


    public List<GameObject> pathWayPoints = new List<GameObject>();
    private bool searchedWayPoints = false;

    Transform target;

    NavMeshAgent agent;

    Animator animator;

    private float targetSpeed;
    private float currentSpeed;
    float speedSmoothTime = 0.3f;
    float speedSmoothVelocity;
    float extraRotationSpeed = 3;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        
        state = State.pathing;


        targetSpeed = 1f;
    }

    

    void Update()
    {
        if (!searchedWayPoints)
        {
            SearchWayPoints();
            searchedWayPoints = true;
        }

        extraRotation();

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= chaseRadius)
        {
            state = State.chaseTarget;
        }

        if (state == State.chaseTarget && distance > chaseRadius)
        {
            agent.isStopped = true;
            state = State.idle;
        }

            switch (state)
        {
            case State.idle:
                Idle();
                break;
            case State.pathing:
                Pathing();
                break;
            case State.chaseTarget:
                ChaseTarget();
                break;
        }

        if (distance <= agent.stoppingDistance)
        {
            FaceTarget();
        }

        // Animation
        //animator.SetFloat("speedPercent", new Vector2(agent.velocity.x, agent.velocity.z).magnitude);
    }

    void SearchWayPoints()
    {
        GameObject[] foundWayPoints = GameObject.FindGameObjectsWithTag("WayPoint");

        foreach (GameObject wayPoint in foundWayPoints)
        {
            if (Vector3.Distance(wayPoint.transform.position, transform.position) < wayPointSearchRadius)
            {
                if (wayPoint.name == "WayPoint")
                    pathWayPoints.Add(wayPoint);
            }
        }

        foreach (GameObject wayPoint in foundWayPoints)
        {
            if (Vector3.Distance(wayPoint.transform.position, transform.position) < wayPointSearchRadius)
            {
                if (wayPoint.name == "WayPoint1")
                    pathWayPoints.Add(wayPoint);
            }
        }

        foreach (GameObject wayPoint in foundWayPoints)
        {
            if (Vector3.Distance(wayPoint.transform.position, transform.position) < wayPointSearchRadius)
            {
                if (wayPoint.name == "WayPoint2")
                    pathWayPoints.Add(wayPoint);
            }
        }

        foreach (GameObject wayPoint in foundWayPoints)
        {
            if (Vector3.Distance(wayPoint.transform.position, transform.position) < wayPointSearchRadius)
            {
                if (wayPoint.name == "WayPoint3")
                    pathWayPoints.Add(wayPoint);
            }
        }
    }



    void Idle()
    {
        targetSpeed = 0;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        agent.speed = currentSpeed;

        float animationSpeedPercent = currentSpeed / 4f;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            timer = 0;
            state = State.pathing;
            agent.isStopped = false;
        }
    }

    void Pathing()
    {
        
        targetSpeed = 1f;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        agent.speed = currentSpeed;

        float animationSpeedPercent = currentSpeed / 4f;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        
        agent.SetDestination(pathWayPoints[currentWayPoint].transform.position);

        if (Vector3.Distance(transform.position, pathWayPoints[currentWayPoint].transform.position) < 0.2f)
        {
            currentWayPoint++;

            if (currentWayPoint >= pathWayPoints.Count)
            {
                currentWayPoint = 0;
            }
        }
    }

    void ChaseTarget()
    {
        
        targetSpeed = 4f;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        agent.speed = currentSpeed;

        float animationSpeedPercent = currentSpeed / 4f;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        
        animator.SetFloat("speedPercent", 1f);
        timer = 0;
        agent.SetDestination(target.position);
        agent.isStopped = false;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    

    void extraRotation()
    {
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
