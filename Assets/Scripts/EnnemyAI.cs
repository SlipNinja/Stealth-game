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
    private List<Transform> footprintsInView;
    private Vector3 lastSeenPlayerPosition;

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
        footprintsInView = new List<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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

            float currDist;
            float farthestDistance = 0f;
            Transform farthest = footprintsInView[0];

            foreach (Transform footp in footprintsInView)
            {
                //currDist = Vector3.Distance(footp.position, lastSeenPlayerPosition);
                currDist = Vector3.Distance(footp.position, transform.position);
                if( currDist > farthestDistance)
                {
                    farthest = footp;
                    farthestDistance = currDist;
                }
            }

            target = farthest;
        }
        else if (attacking)
        {
            navmesh.speed = attackSpeed;
            range = attackRange;
            angle = attackAngle;

            target = player;
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
                        //lastSeenPlayerPosition = player.position;
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
        bool hasFootprints = false;
        angleNear = angle * 2;
        footprintsInView.Clear(); // Empty List

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
                            footprintsInView.Add(hit.transform); // Add footprints visible
                            Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                            hasFootprints = true;
                        }
                    }
                }
            }
        }
        return hasFootprints;
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
