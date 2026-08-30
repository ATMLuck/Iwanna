using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Lock2 : MonoBehaviour
{
    public bool isTrigger = false;
    public Collider2D c2d;
    bool Trigger = false;
    void Update()
    {
        if(isTrigger&&!Trigger)
        {
            Trigger = true;
            isTrigger = false;
            MovingPlatform me_component = GetComponent<MovingPlatform>();
            me_component.isMoving = true;
            Spike sp = GetComponent<Spike>();
            sp.enabled = false;
            c2d.isTrigger = false;
        }
        
    }
}
