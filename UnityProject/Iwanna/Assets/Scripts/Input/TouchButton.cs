using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 单次触发按钮：按下瞬间触发一次跳跃/攻击。
/// </summary>
public class TouchButton : MonoBehaviour, IPointerDownHandler
{
    public enum ButtonType { Jump, Shoot }

    public ButtonType type = ButtonType.Jump;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (type == ButtonType.Jump) VirtualInput.PressJump();
        else VirtualInput.PressShoot();
    }

    private void OnDisable()
    {
        if (type == ButtonType.Jump) VirtualInput.ConsumeJump();
        else VirtualInput.ConsumeShoot();
    }
}
