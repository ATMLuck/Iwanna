using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class movingPlatform : MonoBehaviour
{
    [Header("往返端点（相对平台当前位置的偏移）")]
    [SerializeField] private Vector2 offsetA = new Vector2(-3f, 0f);
    [SerializeField] private Vector2 offsetB = new Vector2(3f, 0f);

    [Header("运动参数")]
    [SerializeField, Min(0f)] private float moveSpeed = 2f;
    [SerializeField, Min(0f)] private float waitAtEnds = 0.3f;

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _waitTimer;

    private void Start()
    {
        _startPos = transform.position;
        _targetPos = _startPos + (Vector3)offsetA;   // 起步先往 A 走
    }

    private void Update()
    {
        // 1) 朝目标匀速走
        transform.position = Vector3.MoveTowards(
            transform.position, _targetPos, moveSpeed * Time.deltaTime);

        // 2) 到了端点 → 等一下 → 切方向
        if (Vector2.Distance(transform.position, _targetPos) < 0.01f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitAtEnds)
            {
                _waitTimer = 0f;
                Vector3 posA = _startPos + (Vector3)offsetA;
                Vector3 posB = _startPos + (Vector3)offsetB;
                _targetPos = (Vector2.Distance(_targetPos, posB) < 0.01f) ? posA : posB;
            }
        }
    }
}
