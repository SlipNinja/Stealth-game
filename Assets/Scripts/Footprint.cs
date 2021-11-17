using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    // Start is called before the first frame update
    public int secondsToDestroy = 3;
    void Start()
    {
        Destroy(gameObject, secondsToDestroy);
    }
}
