using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour
{

    public GameObject footstepPrefab;
    public Transform footprints;

    private Vector3 lastfootprint;
    private GameObject footprint;

    // Start is called before the first frame update
    void Start()
    {
        lastfootprint = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float distTo = Vector3.Distance(transform.position, lastfootprint);
        if (distTo > 2f)
        {
            Vector3 instanciatePosition = new Vector3(transform.position.x, 0, transform.position.z);
            footprint = Instantiate(footstepPrefab, instanciatePosition, Quaternion.identity);//Change quaternion to rotate footprint correctly
            footprint.transform.parent = footprints;
            lastfootprint = footprint.transform.position;
        }
    }
}
