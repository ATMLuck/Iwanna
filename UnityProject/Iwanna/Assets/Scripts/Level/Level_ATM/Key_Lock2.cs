using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Lock2 : MonoBehaviour
{
    public bool isTrigger = false;
    bool Trigger = false;
    void Update()
    {
        if(isTrigger&&!Trigger)
        {
            Trigger = true;
            isTrigger = false;
            MovingPlatform me_component = GetComponent<MovingPlatform>();
            me_component.isMoving = true;
        }
        
    }
}
