using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    [Header("移动参数")]
    public float moveSpeed = 7f;
    public float jumpForce = 8f;
    public float doubleJumpForce = 6f;
    public float bulletSpeed = 15f;

    [Header("二段跳设置")]
    public bool canDoubleJump = true;
    private int jumpCount = 0;

    [Header("地面检测")]
    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius = 0.25f;
    public LayerMask groundLayer;

    [Header("死亡设置")]
    public float deathY = -12f;
    private bool isDead = false;

    [Header("射击参数")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private int facingDir = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        GameManager.Instance.RegisterPlayer(this);
    }

    void Update()
    {
        if (isDead) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(InputDef.Jump))
        {
            if (jumpCount == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetTrigger("JumpTrigger");
                jumpCount++;
            }
            else if (jumpCount == 1 && canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                anim.SetTrigger("DoubleJumpTrigger");
                jumpCount++;
            }
        }

        if (Input.GetKeyDown(InputDef.Shoot))
        {
            Shoot();
            anim.SetTrigger("AttackTrigger");
        }

        if (transform.position.y < deathY)
        {
            EventCenter.Broadcast(GameEvent.PlayerDeath);
            Die();
        }

        if (firePoint != null)
        {
            Vector3 fpPos = firePoint.localPosition;
            fpPos.x = Mathf.Abs(fpPos.x) * facingDir;
            firePoint.localPosition = fpPos;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        float dirX = Input.GetAxisRaw(InputDef.Horizontal);
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (dirX != 0)
        {
            facingDir = (int)Mathf.Sign(dirX);
            sprite.flipX = facingDir == -1;

            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = new Vector2(facingDir * bulletSpeed, 0f);
            }
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    public void OnDeathAnimationFinished()
    {
        GameManager.Instance.OnPlayerDeathAnimationFinished();
    }

    public void Respawn(Vector3 respawnPosition)
    {
        transform.position = respawnPosition;
        isDead = false;
        rb.bodyType = RigidbodyType2D.Dynamic;

        anim.ResetTrigger("death");
        anim.ResetTrigger("JumpTrigger");
        anim.ResetTrigger("DoubleJumpTrigger");
        anim.ResetTrigger("AttackTrigger");

        anim.SetBool("running", false);
        anim.SetBool("isGrounded", true);

        anim.Play("Idle", 0, 0f);
        anim.Update(0);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}