using TMPro;
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

    [Header("锁定关卡图标")]
    [SerializeField] private Sprite lockedLevelSprite;
    private Sprite _normalLevelSprite;

    [Header("音量滑动条")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    void Start()
    {
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        InitVolumeSliders();
    }

    /// <summary>
    /// 初始化音量滑动条：用已保存的音量刷新滑动条并应用到 AudioManager
    /// </summary>
    private void InitVolumeSliders()
    {
        if (ProgressManager.Instance == null) return;

        float musicVolume = ProgressManager.Instance.MusicVolume;
        float sfxVolume = ProgressManager.Instance.SFXVolume;

        if (musicVolumeSlider != null)
            musicVolumeSlider.SetValueWithoutNotify(musicVolume);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.SetValueWithoutNotify(sfxVolume);

        AudioManager.Instance.SetMusicVolume(musicVolume);
        AudioManager.Instance.SetSFXVolume(sfxVolume);
    }

    public void OnMusicVolumeChanged(float value)
    {
        ProgressManager.Instance.MusicVolume = value;
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        ProgressManager.Instance.SFXVolume = value;
        AudioManager.Instance.SetSFXVolume(value);
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
        if (levelButtons == null || levelButtons.Length == 0) return;

        // 缓存默认关卡图标，用于锁定/解锁状态切换时恢复
        if (_normalLevelSprite == null && levelButtons[0] != null && levelButtons[0].image != null)
            _normalLevelSprite = levelButtons[0].image.sprite;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] == null) continue;

            int levelNumber = i + 1;
            bool unlocked = ProgressManager.Instance.IsLevelUnlocked(levelNumber);
            levelButtons[i].interactable = unlocked;

            Image bg = levelButtons[i].image;
            if (bg != null)
                bg.sprite = unlocked ? _normalLevelSprite : lockedLevelSprite;

            TextMeshProUGUI numberText = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>(true);
            if (numberText != null)
                numberText.gameObject.SetActive(unlocked);
        }
    }
}