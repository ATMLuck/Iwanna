using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSensor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 向上通知父物体
        SendMessageUpwards("OnSensorTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
    }
}
