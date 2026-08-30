using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 2f;
    private Rigidbody2D rb;
    private Animator ani;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.flipX = rb.velocity.x < 0f;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰到 Ground 层的物体就销毁
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ani.SetBool("Bullet_is_Dead",true);
        }
    }
    public void Dis()
    {
        Destroy(gameObject);
    }
}