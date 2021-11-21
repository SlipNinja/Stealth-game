using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour
{

    public GameObject footstepPrefab;
    public Transform footprints;

    private Vector3 lastfootprint;
    private GameObject footprint;

    private Transform left;
    private Transform right;
    private bool leftFootTime;

    // Start is called before the first frame update
    void Start()
    {
        lastfootprint = new Vector3(0, 0, 0);
        left = transform.Find("leftfoot");
        right = transform.Find("rightfoot");
        leftFootTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distTo = Vector3.Distance(transform.position, lastfootprint);
        if (distTo > 4f)
        {
            Vector3 instanciatePosition = Vector3.zero;
            RaycastHit hit;
            //float playerAngle = Vector3.Angle(targetDir, transform.forward);

            if(leftFootTime)//left foot
            {
                if(Physics.Raycast(left.position, Vector3.down, out hit, 100f))
                {
                    instanciatePosition = hit.point;
                    leftFootTime = false;
                }
            } else {//right foot
                if(Physics.Raycast(right.position, Vector3.down, out hit, 100f))
                {
                    instanciatePosition = hit.point;
                    leftFootTime = true;
                }
            }

            footprint = Instantiate(footstepPrefab, instanciatePosition, Quaternion.identity);
            footprint.transform.rotation = Quaternion.FromToRotation (footprint.transform.up, hit.normal) * footprint.transform.rotation;
            footprint.transform.Rotate(0, transform.eulerAngles.y, 0);//Rotate footprints
            footprint.transform.parent = footprints;
            lastfootprint = footprint.transform.position;
        }
    }
}
