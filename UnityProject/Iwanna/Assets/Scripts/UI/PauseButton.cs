using UnityEngine;
using UnityEngine.EventSystems;

// 挂在 HUD 暂停图标上，点击时暂停游戏。
public class PauseButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.PauseGame();
    }
}
