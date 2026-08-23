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
    }

    void Update()
    {
        if (isDead) return;

        // 1. 地面检测
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // 2. 跳跃
        if (Input.GetKeyDown(InputDef.Jump))
        {
            if (jumpCount == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
            }
            else if (jumpCount == 1 && canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                jumpCount++;
            }
        }

        // 3. 开枪
        if (Input.GetKeyDown(InputDef.Shoot))
        {
            Shoot();
        }

        // 4. 虚空死亡
        if (transform.position.y < deathY)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // 水平移动
        float dirX = Input.GetAxisRaw(InputDef.Horizontal);
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (dirX != 0)
        {
            facingDir = (int)Mathf.Sign(dirX);
            transform.localScale = new Vector3(facingDir, 1, 1);
            anim.SetBool("running", true);
            firePoint.localPosition = new Vector3(
                Mathf.Abs(firePoint.localPosition.x) * facingDir,
                firePoint.localPosition.y,
                0f
            );
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
        StartCoroutine(RestartAfterDelay());
    }

    public IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        int idx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(idx);
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