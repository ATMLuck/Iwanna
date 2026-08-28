using UnityEngine;

// 挂载到一个新建的空物体上（比如叫 CameraTarget），放在角色附近
// 作用：持续跟随玩家位置，作为摄像机真正跟随的"中间目标"
// 好处：以后想给摄像机加预判、抖动、镜头偏移等效果，只需要改这一个物体，
// 不用碰 PlayerController 也不用碰 CameraFollow 本身
public class PlayerFollowTarget : MonoBehaviour
{
    [Header("跟随目标 (留空则自动查找 Tag 为 Player 的物体)")]
    public Transform player;

    [Header("跟随参数")]
    public Vector2 offset = Vector2.zero;   // 相对玩家的偏移，比如想让目标点略高于角色头顶
    public float smoothTime = 0.1f;         // 平滑跟随耗时，0表示完全贴合不做平滑

    private Vector2 _velocity;

    void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (player != null)
        {
            // 开局直接对齐到玩家位置，避免第一帧从物体摆放的初始位置平滑过来产生跳动
            transform.position = (Vector2)player.position + offset;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector2 desired = (Vector2)player.position + offset;
        if (smoothTime <= 0f)
        {
            transform.position = desired;
        }
        else
        {
            Vector2 current = transform.position;
            Vector2 smoothed = Vector2.SmoothDamp(current, desired, ref _velocity, smoothTime);
            transform.position = smoothed;
        }
    }
}