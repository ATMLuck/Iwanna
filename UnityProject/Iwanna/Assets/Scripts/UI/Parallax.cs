using UnityEngine;

// 挂载到关卡场景里每一层背景图层上（远山、云、树林等）
// 原理：根据这一层在Z轴上离摄像机/离角色的距离，自动算出视差移动速度，
// 不需要像主菜单那个MenuParallax脚本一样每层手动调"Offset Multiplier"
public class Parallax : MonoBehaviour
{
    [Header("引用")]
    public Camera cam;         // 拖入 Main Camera
    public Transform subject;  // 拖入角色（Player），作为"视差基准面"

    private Vector2 startPosition;    // 这一层最初摆放的位置
    private float startZ;             // 这一层最初的Z深度，全程保持不变
    private Vector2 camStartPosition; // 摄像机自己最初的位置，作为Travel的正确基准

    // 摄像机从游戏开始到现在，一共移动了多少
    private Vector2 Travel => (Vector2)cam.transform.position - camStartPosition;

    // 这一层相对于角色（视差基准面）在Z轴上的距离，正=在角色后面(背景)，负=在角色前面(前景)
    private float DistanceFromSubject => transform.position.z - subject.position.z;

    // 根据距离方向，选用摄像机的远裁剪面或者近裁剪面作为参照深度范围
    private float ClippingPlane =>
        cam.transform.position.z + (DistanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane);

    // 视差系数：离摄像机越远（占裁剪范围比例越大），移动速度越接近摄像机本身；
    // 离角色越近，系数越接近0，几乎不随摄像机移动
    private float ParallaxFactor => Mathf.Abs(DistanceFromSubject) / ClippingPlane;

    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
        camStartPosition = cam.transform.position;
    }

    void Update()
    {
        Vector2 newPos = startPosition + Travel * ParallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}