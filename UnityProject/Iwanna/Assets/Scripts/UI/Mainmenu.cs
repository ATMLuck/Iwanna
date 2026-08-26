using UnityEngine;
using UnityEngine.SceneManagement;

// 挂载到 Canvas 物体上（或者随便一个常驻的空物体上），负责主菜单按钮的跳转逻辑
public class MainMenu : MonoBehaviour
{
    // 绑定到"开始游戏"按钮的 OnClick 事件
    public void Play()
    {
        // 括号里的"Game"要换成你自己游戏关卡场景的名字，并且这个场景必须已经加到
        // File → Build Settings 的 Scenes In Build 列表里，否则会报错找不到场景
        SceneManager.LoadScene("Level_04");
    }

    // 绑定到"退出游戏"按钮的 OnClick 事件
    public void Quit()
    {
        // 注意：这个方法只有在打包后的正式游戏里才会真正退出程序
        // 在Unity编辑器里点击测试是没有效果的，这是正常现象，不是bug
        Application.Quit();
    }
}