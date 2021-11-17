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
        if (distTo > 1.5f)
        {
            Vector3 instanciatePosition;

            if(leftFootTime)//left foot
            {
                instanciatePosition = new Vector3(left.position.x, 0, left.position.z);
                leftFootTime = false;
            } else {//right foot
                instanciatePosition = new Vector3(right.position.x, 0, right.position.z);
                leftFootTime = true;
            }

            footprint = Instantiate(footstepPrefab, instanciatePosition, Quaternion.identity);
            footprint.transform.Rotate(0, transform.eulerAngles.y, 0);//Rotate footprints
            footprint.transform.parent = footprints;
            lastfootprint = footprint.transform.position;
        }
    }
}
