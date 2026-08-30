using UnityEngine;
using UnityEngine.SceneManagement;

// 挂载到 Bootstrap 物体上，持续监听ESC键，切换暂停/继续
// 只在非主菜单场景生效，避免在主菜单界面按ESC也触发暂停逻辑
public class PauseInputHandler : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    void Update()
    {
        if (SceneManager.GetActiveScene().name == mainMenuSceneName) return;

        if (Input.GetKeyDown(InputDef.Pause))
        {
            if (Time.timeScale > 0f)
            {
                GameManager.Instance.PauseGame();
            }
            else
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }
}