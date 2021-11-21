using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAI : MonoBehaviour
{
    public float patrolSpeed;
    public float searchSpeed;
    public float attackSpeed;
    public float patrolRange;
    public float searchRange;
    public float attackRange;
    public float patrolAngle;
    public float searchAngle;
    public float attackAngle;
    
    public Transform target;
    public Transform[] destinations;
    private float range, angle, angleNear, rangeNear;
    public Transform player;
    public Transform footprints;
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
        rangeNear = 4f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(!target)
        {
            currentDestination = (currentDestination + 1 >= destinations.Length) ? 0 : currentDestination + 1;
            target = destinations[currentDestination];
        }
        
        navmesh.destination = target.position;// Moving

        if(patrolling)
        {
            navmesh.speed = patrolSpeed;
            range = patrolRange;
            angle = patrolAngle;

            float distTo = Vector3.Distance(transform.position, target.position);
            if(distTo <= 1f)//ppoint reached
            {
                currentDestination = (currentDestination + 1 >= destinations.Length) ? 0 : currentDestination + 1;
                target = destinations[currentDestination];
            }
        }
        else if(searching)
        {
            navmesh.speed = searchSpeed;
            range = searchRange;
            angle = searchAngle;

        }
        else if (attacking)
        {
            navmesh.speed = attackSpeed;
            range = attackRange;
            angle = attackAngle;

            target = player;
        }

        if(PlayerInView())
        {
            patrolling = false;
            searching = false;
            attacking = true;
        }
        else if(FootprintInView())
        {
            patrolling = false;
            attacking = false;
            searching = true;
        }
        else
        {
            patrolling = true;
            searching = false;
            attacking = false;
        }
    }

    private bool PlayerInView()
    {
        if(Vector3.Distance(transform.position, player.position) < range)
        {
            Vector3 targetDir = player.position - transform.position;
            float playerAngle = Vector3.Angle(targetDir, transform.forward);

            if (playerAngle <= angle)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, targetDir, out hit, range, 7))
                {
                    if(hit.transform.name.Contains("player"))
                    {
                        Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool FootprintInView()
    {
        float dist;
        bool near = false;
        angleNear = angle * 2;

        foreach(Transform child in footprints)
        {
            dist = Vector3.Distance(transform.position, child.position);
            if( dist < rangeNear){
                near = true;
            }
            if(( dist < range) || near)
            {
                Vector3 targetDir = child.position - transform.position;
                float footprintAngle = Vector3.Angle(targetDir, transform.forward);

                if ((footprintAngle <= angle) || (near && (footprintAngle <= angleNear)))
                {
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, targetDir, out hit, range, 7))
                    {
                        if(hit.transform.name.Contains("Footprint"))
                        {
                            target = hit.transform;
                            Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 direction = transform.TransformDirection(Vector3.forward);

        Vector3 leftdir = Quaternion.AngleAxis(angle, Vector3.up) * direction * range;
        Vector3 rightdir = Quaternion.AngleAxis(-angle, Vector3.up) * direction * range;

        Vector3 leftdirnear = Quaternion.AngleAxis(angleNear, Vector3.up) * direction * rangeNear;
        Vector3 rightdirnear = Quaternion.AngleAxis(-angleNear, Vector3.up) * direction * rangeNear;

        //Gizmos.DrawRay(transform.position, direction);
        Gizmos.DrawRay(transform.position, leftdir);
        Gizmos.DrawRay(transform.position, rightdir);
        Gizmos.DrawRay(transform.position, leftdirnear);
        Gizmos.DrawRay(transform.position, rightdirnear);

        
    }

        //transform.LookAt(target);
        //Vector3 moveTo = Vector3.MoveTowards(transform.position, target.position, 5000f);
        //float playerAngle = Quaternion.Angle(Quaternion.AngleAxis(30, Vector3.up), Quaternion.AngleAxis(0, Vector3.up));



}
