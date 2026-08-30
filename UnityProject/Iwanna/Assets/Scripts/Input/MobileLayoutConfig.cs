using System;
using UnityEngine;

// 移动端触摸按钮的布局配置。
// 由编辑器工具（Tools/MobileUI）生成/保存，运行时 MobileInput 读取。
// 放在 Resources 下以支持 Resources.Load 加载。
[CreateAssetMenu(fileName = "MobileLayoutConfig", menuName = "MobileUI/布局配置")]
public class MobileLayoutConfig : ScriptableObject
{
    public enum Anchor { BottomLeft, BottomRight }

    [Serializable]
    public class ButtonLayout
    {
        public string id;                                 // RightMove / LeftMove / Jump / Attack
        public Anchor anchor = Anchor.BottomLeft;         // 锚定在哪个角
        public Vector2 anchoredPosition;                  // 相对锚点的位置
        public Vector2 size = new Vector2(180f, 180f);    // 按钮尺寸
    }

    public ButtonLayout[] buttons = new ButtonLayout[0];

    public ButtonLayout Get(string id)
    {
        if (buttons == null) return null;
        foreach (var b in buttons)
            if (b != null && b.id == id) return b;
        return null;
    }
}
