using UnityEngine;
using UnityEngine.EventSystems;

// 移动按钮：按住持续移动，松手停止（左/右各挂一个）。
public class HoldMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("移动方向：-1 左，1 右")]
    public int direction = 1;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (direction < 0) VirtualInput.MoveLeftHeld = true;
        else VirtualInput.MoveRightHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (direction < 0) VirtualInput.MoveLeftHeld = false;
        else VirtualInput.MoveRightHeld = false;
    }

    private void OnDisable()
    {
        // 按钮被隐藏/禁用时避免按住状态卡住
        if (direction < 0) VirtualInput.MoveLeftHeld = false;
        else VirtualInput.MoveRightHeld = false;
    }
}
