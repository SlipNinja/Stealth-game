using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform[] destinations;
    public float range, angle, height;
    public GameObject player;
    private NavMeshAgent navmesh;
    private float timer;
    private int currentDestination;
    private bool patrolling, searching, attacking;

    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        currentDestination = 0;
        patrolling = true;
        searching = false;
        attacking = false;
        target = destinations[currentDestination];
        //range = 50f;
        //angle = 30f;
        //height = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(patrolling)
        {
            navmesh.destination = target.position;
            float distTo = Vector3.Distance(transform.position, target.position);
            if(distTo <= 1f)//ppoint reached
            {
                currentDestination = (currentDestination + 1 >= destinations.Length) ? 0 : currentDestination + 1;
            }

            target = destinations[currentDestination];
        } else if(searching)
        {

        } else if (attacking)
        {

        }

        if(PlayerInView())
        {
            Debug.Log("PLAYER VISIBLE");
        } else {
            Debug.Log("NOT SEEN");
        }
    }

    bool PlayerInView()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < range)
        {
            Vector3 targetDir = player.transform.position - transform.position;
            float playerAngle = Vector3.Angle(targetDir, transform.forward);

            if (playerAngle <= angle)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, targetDir, out hit, range, 7))
                {
                    if(hit.transform.name == "Player")
                    {
                        Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * range;

        Vector3 leftdir = Quaternion.AngleAxis(angle, Vector3.up) * direction;
        Vector3 rightdir = Quaternion.AngleAxis(-angle, Vector3.up) * direction;

        //Gizmos.DrawRay(transform.position, direction);
        Gizmos.DrawRay(transform.position, leftdir);
        Gizmos.DrawRay(transform.position, rightdir);

        
    }

        //transform.LookAt(target);
        //Vector3 moveTo = Vector3.MoveTowards(transform.position, target.position, 5000f);
        //float playerAngle = Quaternion.Angle(Quaternion.AngleAxis(30, Vector3.up), Quaternion.AngleAxis(0, Vector3.up));



}
