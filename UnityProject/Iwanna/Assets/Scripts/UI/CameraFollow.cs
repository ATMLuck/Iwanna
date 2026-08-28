using UnityEngine;

// 挂载到 Main Camera 上，负责摄像机跟随移动，并可选限制在关卡范围内
public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标 (拖入 CameraTarget 物体)")]
    public Transform target;

    [Header("跟随参数")]
    public float smoothTime = 0.15f;
    public Vector3 offset = new Vector3(0f, 1f, -10f);

    [Header("地图边界限制 - 方式一：拖入圈出关卡范围的Collider2D，自动计算（推荐）")]
    public Collider2D levelBounds;

    [Header("地图边界限制 - 方式二：手动填写边界数值（未拖入方式一时生效）")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 _velocity = Vector3.zero;
    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();

        if (target != null)
        {
            Vector3 snapPosition = target.position + offset;
            transform.position = ClampToBounds(snapPosition);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothTime);

        transform.position = ClampToBounds(smoothedPosition);
    }

    private Vector3 ClampToBounds(Vector3 pos)
    {
        if (levelBounds != null)
        {
            // 自动计算：把镜头的半宽半高也纳入考虑，
            // 保证镜头边缘不会超出关卡范围，而不是只把镜头中心点限制住
            Bounds b = levelBounds.bounds;
            float halfHeight = _cam.orthographicSize;
            float halfWidth = halfHeight * _cam.aspect;

            float minX = b.min.x + halfWidth;
            float maxX = b.max.x - halfWidth;
            float minY = b.min.y + halfHeight;
            float maxY = b.max.y - halfHeight;

            // 如果关卡本身比镜头视野还小，min会大于max，这时候直接定在关卡中心，避免抖动
            pos.x = minX <= maxX ? Mathf.Clamp(pos.x, minX, maxX) : b.center.x;
            pos.y = minY <= maxY ? Mathf.Clamp(pos.y, minY, maxY) : b.center.y;
        }
        else if (useBounds)
        {
            pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
            pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        }

        return pos;
    }
}