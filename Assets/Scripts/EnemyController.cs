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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        
        state = State.pathing;
    }

    

    void Update()
    {
        if (!searchedWayPoints)
        {
            SearchWayPoints();
            searchedWayPoints = true;
        }

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
        animator.SetFloat("speedPercent", new Vector2(agent.velocity.x, agent.velocity.z).magnitude);
    }

    void SearchWayPoints()
    {
        GameObject[] foundWayPoints = GameObject.FindGameObjectsWithTag("WayPoint");

        foreach (GameObject wayPoint in foundWayPoints)
        {
            if (Vector3.Distance(wayPoint.transform.position, transform.position) < wayPointSearchRadius)
            {
                pathWayPoints.Add(wayPoint);
            }
        }
    }



    void Idle()
    {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
