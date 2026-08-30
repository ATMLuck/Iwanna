using UnityEngine;

/// <summary>
/// 触摸输入共享状态：PC 键鼠与移动端触摸按钮共用同一输入通道。
/// </summary>
public static class VirtualInput
{
    public static bool MoveLeftHeld;
    public static bool MoveRightHeld;

    // 水平轴合成：左右同时按下时归零
    public static float Horizontal
    {
        get
        {
            if (MoveLeftHeld == MoveRightHeld) return 0f;
            return MoveRightHeld ? 1f : -1f;
        }
    }

    private static bool _jumpPressed;
    private static bool _shootPressed;

    public static void PressJump() => _jumpPressed = true;
    public static void PressShoot() => _shootPressed = true;

    // 单次读取：返回 true 并复位（等价 GetKeyDown 语义）
    public static bool ConsumeJump()
    {
        bool value = _jumpPressed;
        _jumpPressed = false;
        return value;
    }

    public static bool ConsumeShoot()
    {
        bool value = _shootPressed;
        _shootPressed = false;
        return value;
    }

    public static void Reset()
    {
        MoveLeftHeld = false;
        MoveRightHeld = false;
        _jumpPressed = false;
        _shootPressed = false;
    }
}
