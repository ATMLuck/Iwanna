using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class InputDef
{
    public const string Horizontal = "Horizontal"; // 移动轴，用 Input.GetAxisRaw 读取（无平滑，更跟手）
    public const KeyCode Jump  = KeyCode.Space;    // 跳跃（二段跳）
    public const KeyCode Shoot = KeyCode.J;        // 发射子弹
    public const KeyCode Pause = KeyCode.Escape;   // 暂停/返回主菜单
}