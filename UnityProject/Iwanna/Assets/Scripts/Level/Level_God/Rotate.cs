using UnityEngine;

public class RotateAndMove : MonoBehaviour
{
    [Header("目标设置")]
    [SerializeField] private Vector3 targetPosition = new Vector3(5f, 0f, 5f);
    [SerializeField] private float arrivalDistance = 0.05f;

    [Header("移动与旋转")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 180f;      // 度/秒
    [SerializeField] private bool clockwise = true;         // true=顺时针，false=逆时针
    [SerializeField] private bool keepRotatingAfterArrival = false;

    [Header("到达回调（可接 EventCenter）")]
    public System.Action OnArrived;

    private bool isActive = false;
    private bool hasArrived = false;

    private void Update()
    {
        if (!isActive) return;

        if (hasArrived)
        {
            if (keepRotatingAfterArrival) Spin();
            return;
        }

        Move();
        Spin();

        if (Vector3.Distance(transform.position, targetPosition) <= arrivalDistance)
        {
            hasArrived = true;
            transform.position = targetPosition;
            OnArrived?.Invoke();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void Spin()
    {
        // —— 核心改动：绕 Z 轴转，屏幕上看就是顺/逆时针 ——
        float angle = rotateSpeed * Time.deltaTime;
        if (clockwise) angle = -angle;   // Unity 里正 Z 角是逆时针，负才是顺时针
        transform.Rotate(0f, 0f, angle);
    }

    public void OnSensorTriggerEnter(Collider2D other)
    {
        isActive = true;
        hasArrived = false;
    }

    public void SetTarget(Vector3 newTarget)
    {
        targetPosition = newTarget;
        hasArrived = false;
        isActive = false;
    }

    private void OnEnable() => EventCenter.Subscribe(GameEvent.PlayerRespawned, Restore);
    private void OnDisable() => EventCenter.Unsubscribe(GameEvent.PlayerRespawned, Restore);

    // 运行时切方向，方便调试
    public void SetClockwise(bool isClockwise) => clockwise = isClockwise;
    // —— ① 在字段声明区（Awake 里缓存初始位姿）——
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // —— ② 加这个 Restore 函数，其他一概不动 ——
    public void Restore(object arg)
    {
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        isActive = false;
        hasArrived = false;
    }

}
