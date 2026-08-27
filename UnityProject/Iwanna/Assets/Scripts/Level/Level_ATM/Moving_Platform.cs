using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(4f, 0f, 0f);
    [SerializeField] private float speed = 3f;

    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 target;
    public bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + offset;
        target = endPos;
    }

    private void FixedUpdate()
    {
        if(isMoving)
        {
            Vector3 next = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(next);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                target = (target == startPos) ? endPos : startPos;
            }
        }
        
    }
}
