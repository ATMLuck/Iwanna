using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InputDef
{
    public const string Horizontal = "Horizontal";
    public const string Jump = "Jump";
    public const KeyCode Shoot = KeyCode.J;
}

public enum PlayerState
{
    Idle,
    Running,
    Jumping
}

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

    public PlayerState CurrentState { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        CurrentState = PlayerState.Idle;
    }

    void Update()
    {
        if (isDead) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetButtonDown(InputDef.Jump))
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

        if (Input.GetKeyDown(InputDef.Shoot))
        {
            Shoot();
        }

        if (transform.position.y < deathY)
        {
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

        UpdatePlayerState(dirX);
    }

    private void UpdatePlayerState(float dirX)
    {
        if (!isGrounded)
        {
            CurrentState = PlayerState.Jumping;
        }
        else if (dirX != 0)
        {
            CurrentState = PlayerState.Running;
        }
        else
        {
            CurrentState = PlayerState.Idle;
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