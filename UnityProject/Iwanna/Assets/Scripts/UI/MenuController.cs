using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("主菜单按钮组 (Start / Options / Quit，不含标题)")]
    public GameObject mainButtonsGroup;

    [Header("选关面板 (4个关卡按钮)")]
    public GameObject levelSelectPanel;

    [Header("关卡按钮列表 (按顺序拖入：第0个对应第1关，第1个对应第2关，以此类推)")]
    public Button[] levelButtons;

    void Start()
    {
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
    }

    public void OnStartClicked()
    {
        if (mainButtonsGroup != null) mainButtonsGroup.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
        RefreshLevelButtons();
    }

    public void OnBackClicked()
    {
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (mainButtonsGroup != null) mainButtonsGroup.SetActive(true);
    }

    // 新增：关卡按钮统一调用这个方法，而不是直接绑定 Bootstrap 上的 GameManager.LoadLevel
    // 这里在真正点击的那一刻，才去动态获取当前有效的 GameManager.Instance，
    // 不会受"哪个 Bootstrap 实例被销毁"影响
    public void LoadLevel(int levelNumber)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadLevel(levelNumber);
        }
        else
        {
            Debug.LogWarning("GameManager.Instance 为空，无法加载关卡");
        }
    }

    private void RefreshLevelButtons()
    {
        if (levelButtons == null) return;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] == null) continue;

            int levelNumber = i + 1;
            bool unlocked = ProgressManager.Instance.IsLevelUnlocked(levelNumber);
            levelButtons[i].interactable = unlocked;
        }
    }
}