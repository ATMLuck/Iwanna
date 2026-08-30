using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 挂在 HUD 暂停图标上，点击时暂停游戏。
/// </summary>
public class PauseButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.PauseGame();
    }
}
