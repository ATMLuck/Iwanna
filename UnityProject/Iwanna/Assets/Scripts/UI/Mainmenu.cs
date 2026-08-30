using UnityEngine;
using UnityEngine.SceneManagement;

// 挂载到 Canvas 物体上（或者随便一个常驻的空物体上），负责主菜单按钮的跳转逻辑
public class MainMenu : MonoBehaviour
{
    // 绑定到"开始游戏"按钮的 OnClick 事件
    public void Play()
    {
        SceneManager.LoadScene("Level_04");
    }

    // 绑定到"退出游戏"按钮的 OnClick 事件
    public void Quit()
    {
        // 注意：这个方法只有在打包后的正式游戏里才会真正退出程序
        Application.Quit();
    }
}