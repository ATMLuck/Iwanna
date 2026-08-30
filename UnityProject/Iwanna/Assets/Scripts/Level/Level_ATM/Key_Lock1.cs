using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Key_Lock1 : MonoBehaviour
{
    public bool isTrigger = false;
    
    void FixedUpdate()
    {
        if(isTrigger)
        {
            transform.Translate(new Vector3(0,-1,0)*Time.deltaTime,Space.World);
        }
    }
}
