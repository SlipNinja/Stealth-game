using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour
{
    public AK.Wwise.Event MyEvent;
    public AK.Wwise.Event Growl;
    public AK.Wwise.Event Sniff;
    // Start is called before the first frame update
    public void FOOTSTEPS()
       

    {
        MyEvent.Post(gameObject);
        
    }
    public void GROWL()

    {
        Growl.Post(gameObject);
    }
    public void SNIFF()

    {
        Sniff.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
