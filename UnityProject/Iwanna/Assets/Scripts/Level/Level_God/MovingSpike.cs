using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 Position;
    [Header("速度控制")]
    public float Rspeed=50;
    public float upspeed=0;
    public float Limit_y=0;
    public float Limit_x=0;

    [Header("移动到的位置")]
    public float position_x=0;
    public float position_y=0;

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
            rb.velocity = new Vector2(Rspeed, upspeed);
        }
        if (gameObject.CompareTag("Moving"))
        {
            transform.position = new Vector3(position_x, position_y, 0);
        }
    }

    void Update()
    {
        if (transform.position.y > Limit_y)
        {
            rb.velocity = new Vector2(0, 0);
        }
        
        if (transform.position.x > Limit_x)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void Restore(object arg)
    {
        rb.velocity=new Vector2(0, 0);
        transform.position = Position;
    }
}
