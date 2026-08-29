using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// 全局常驻 UI 管理器 (负责关卡内叠加 UI：HUD、暂停面板、通关提示、终极通关UI)
/// 架构分工：C 角色 (UI + 美术)
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("UI 面板引用 (挂载在 Canvas 下)")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject clearHintPanel;
    [SerializeField] private GameObject completeUIPanel;

    [Header("HUD 控件 (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI deathCountText;

    [Header("HUD 控件 (Legacy Text 备用)")]
    [SerializeField] private UnityEngine.UI.Text timerTextLegacy;
    [SerializeField] private UnityEngine.UI.Text deathCountTextLegacy;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
        EventCenter.Subscribe(GameEvent.TimerTick, OnTimerTick);
        EventCenter.Subscribe(GameEvent.DeathCountChanged, OnDeathCountChanged);
    }

    private void Start()
    {
        // 初始化面板默认显隐状态
        HidePauseMenu();
        if (clearHintPanel != null) clearHintPanel.SetActive(false);
        if (completeUIPanel != null) completeUIPanel.SetActive(false);
    }

    #region 架构文档规定的对外接口 (由 GameManager 调用)

    /// <summary>
    /// 显示右上角 HUD (进关卡 / 恢复游戏)
    /// </summary>
    public void ShowHUD()
    {
        
        if (hudPanel != null)
        {
            hudPanel.SetActive(true);
        }
            
    }

    /// <summary>
    /// 隐藏右上角 HUD (暂停 / 切场景)
    /// </summary>
    public void HideHUD()
    {
        if (hudPanel != null)
            hudPanel.SetActive(false);
    }

    /// <summary>
    /// 显示暂停面板 (GameManager.PauseGame() 调用)
    /// </summary>
    public void ShowPauseMenu()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    /// <summary>
    /// 隐藏暂停面板 (GameManager.ResumeGame() 调用)
    /// </summary>
    public void HidePauseMenu()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    /// <summary>
    /// 显示"通关！"简短提示 (非最后一关通关时由 GameManager 调用)
    /// </summary>
    public void ShowClearHint()
    {
        if (clearHintPanel != null)
            clearHintPanel.SetActive(true);
    }
    public void HideClearHint()
    {
        if (clearHintPanel != null)
            clearHintPanel.SetActive(false);
    }

    /// <summary>
    /// 显示通关 UI (最后一关通关，含返回主菜单按钮)
    /// </summary>
    public void ShowCompleteUI()
    {
        if (completeUIPanel != null)
            completeUIPanel.SetActive(true);
    }
    public void HideCompleteUI()
    {
        if (completeUIPanel != null)
            completeUIPanel.SetActive(false);
    }

    #endregion

    #region 事件广播回调

    /// <summary>
    /// 刷新计时器 (响应 GameEvent.TimerTick 事件)
    /// </summary>
    private void OnTimerTick(object arg)
    {
        if (arg is float elapsedTime)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100f) % 100f);

            string timeStr = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);

            if (timerText != null) timerText.text = timeStr;
            if (timerTextLegacy != null) timerTextLegacy.text = timeStr;
        }
    }

    /// <summary>
    /// 刷新死亡次数 (响应 GameEvent.DeathCountChanged 事件)
    /// </summary>
    private void OnDeathCountChanged(object arg)
    {
        if (arg is int count)
        {
            string deathStr = $"Deaths: {count}";

            if (deathCountText != null) deathCountText.text = deathStr;
            if (deathCountTextLegacy != null) deathCountTextLegacy.text = deathStr;
        }
    }

    #endregion
}