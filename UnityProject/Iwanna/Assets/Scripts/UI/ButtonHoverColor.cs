using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 挂载到按钮本体上（跟 Button 组件同一个物体）
// 作用：鼠标悬停时，同时改变两个边框装饰图片的颜色；移出时恢复原色
public class ButtonHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("需要变色的两个边框图片（对应你按钮下面的 Image / Image (1)）")]
    public Image borderImage1;
    public Image borderImage2;

    [Header("颜色设置")]
    public Color normalColor = Color.white;
    public Color highlightedColor = new Color(1f, 0.85f, 0.4f); // 默认给个金色

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColor(highlightedColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetColor(normalColor);
    }

    void SetColor(Color color)
    {
        if (borderImage1 != null) borderImage1.color = color;
        if (borderImage2 != null) borderImage2.color = color;
    }
}