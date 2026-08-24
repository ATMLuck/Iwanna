using UnityEngine;

// 挂载到主菜单背景的每一层图层上（比如 MountainsFar、Clouds、TreesBack 等）
// 每一层的 Offset Multiplier 数值不同，就会产生"近处动得多、远处动得少"的视差效果
public class MenuParallax : MonoBehaviour
{
    [Header("视差强度：正数=跟随鼠标同方向移动，负数=反方向移动")]
    public float offsetMultiplier = 1f;

    [Header("跟随的平滑时间：越小跟手越快，越大越有拖尾惯性感")]
    public float smoothTime = .3f;

    private Vector2 startPosition; // 记录这一层最初摆放的位置
    private Vector3 velocity;      // SmoothDamp内部使用，不用手动改

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // 把鼠标屏幕坐标换算成 -0.5~0.5 范围内的偏移量（屏幕中心为0）
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // 让这一层朝着"初始位置 + 偏移量*强度系数"的目标点平滑移动
        transform.position = Vector3.SmoothDamp(
            transform.position,
            startPosition + (offset * offsetMultiplier),
            ref velocity,
            smoothTime
        );
    }
}