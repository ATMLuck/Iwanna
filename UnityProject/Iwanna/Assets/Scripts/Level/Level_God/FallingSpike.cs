using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 Position;

    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        Position= transform.position;
    }

    private void OnEnable() => EventCenter.Subscribe(GameEvent.PlayerRespawned, Restore);
    private void OnDisable() => EventCenter.Unsubscribe(GameEvent.PlayerRespawned, Restore);

    private void OnSensorTriggerEnter(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void Restore(object arg)
    {
        transform.position=Position;
    }
}
