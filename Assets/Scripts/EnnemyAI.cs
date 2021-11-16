using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform[] destinations;

    private NavMeshAgent navmesh;
    private float timer;
    private int currentDestination;
    private bool patrolling;

    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        currentDestination = 0;
        patrolling = true;
        target = destinations[currentDestination];
    }

    // Update is called once per frame
    void Update()
    {
        if(patrolling)
        {
            Move();
            float distTo = Vector3.Distance(transform.position, target.position);
            //Debug.Log(distTo);
            if(distTo <= 1f)//ppoint reached
            {
                //Debug.Log("Point reached");
                currentDestination = (currentDestination + 1 >= destinations.Length) ? 0 : currentDestination + 1;
                Debug.Log("New dest index : " + currentDestination);
            }

            target = destinations[currentDestination];
        }

        
    }

    void Move()
    {
        //transform.LookAt(target);
        Vector3 moveTo = Vector3.MoveTowards(transform.position, target.position, 5000f);
        navmesh.destination = target.position;
    }
}
