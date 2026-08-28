using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 Position;
    public float speed=50;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Position = transform.position;
    }

    private void OnEnable() => EventCenter.Subscribe(GameEvent.PlayerRespawned, Restore);
    private void OnDisable() => EventCenter.Unsubscribe(GameEvent.PlayerRespawned, Restore);

    private void OnSensorTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private void Restore(object arg)
    {
        transform.position = Position;
    }
}
