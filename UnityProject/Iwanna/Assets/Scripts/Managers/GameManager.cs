using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement; 
// enum BGM
// {
//     MainMenu = 0,
//     Level = 1
// }

public class GameManager : Singleton<GameManager>
{
    //===============================字段===================================================
    PlayerController _player;
    int _currentLevel = 1;//当前关卡号
    float _elapsedTime = 0;//本关累计时长（秒)
    int _deathCount = 0;//死亡次数
    int _BGMIndex;//当前BGM序号
    int _MainMenuBGMIndex;//主菜单BGM序号
    Vector3 _lastSavePoint = Vector3.zero;//重生位置
    [Header("通关文字显示时长")]
    [SerializeField] float displayed = 2f;
    // ------------标记------------
    bool _isDead = false;
    bool _loading = false;
    bool _inLevel = false;
    bool _isCompleting = false;

    // ---- 属性（public 只读） ----
    public float ElapsedTime
    {
        get{return _elapsedTime;}
    }
    public int DeathCount
    {
        get{return _deathCount;}
    }
//==============================生命周期函数=============================================
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
        EventCenter.Subscribe(GameEvent.PlayerDeath, OnPlayerDeath);
        EventCenter.Subscribe(GameEvent.SavePointReached, OnSavePoint);
        EventCenter.Subscribe(GameEvent.LevelComplete,OnLevelComplete);
    }
    void Start()
    {
        ProgressManager.Instance.Load();
        _BGMIndex = ProgressManager.Instance.BgmIndex;
        _MainMenuBGMIndex = _BGMIndex;
        AudioManager.Instance.PlayBGM(_BGMIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if(_inLevel&&_loading==false&&Time.timeScale>0)
        {
            _elapsedTime += Time.deltaTime;
            EventCenter.Broadcast(GameEvent.TimerTick, ElapsedTime);
        }
    }
//===============================功能实现===============================================
    
    void OnPlayerDeath(object arg)
    {
        if(_isDead) return;
        _isDead = true;
        _deathCount++;
        EventCenter.Broadcast(GameEvent.DeathCountChanged,DeathCount);
        AudioManager.Instance.PlaySFX(SFXType.PlayerDeath);
        if (_player != null) _player.Die();
    }
    void OnSavePoint(object arg)
    {
       _lastSavePoint = (Vector3)arg;
    }
    void OnLevelComplete(object arg)
    {
        if(_isCompleting) return;
        _isCompleting = true;
        Time.timeScale = 0f;
        if(_currentLevel>=ProgressManager.Instance.TotalLevels)
        {
            UIManager.Instance.ShowCompleteUI();
        }
        else
        {
            UIManager.Instance.ShowClearHint();
            ProgressManager.Instance.UnlockLevel(_currentLevel+1);
            StartCoroutine(PauseLogicCoroutine(displayed));
        }
        
    }
    IEnumerator PauseLogicCoroutine(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        LoadNextLevel();
    }
    void ResetLevelState()//清空计时/死亡次数
    {
        _elapsedTime = 0f;
        _deathCount = 0;
    }


//===============================接口===================================================
    public void LoadLevel(int n)
    {
        if(_loading) return;
        _loading = true;
        _isCompleting = false;
        Time.timeScale = 1f;
        //关卡校验
        if(ProgressManager.Instance.IsLevelUnlocked(n)==false)
        {
            //feat:未解锁提示->UIManager
            _loading = false;
            return;
        }
        //切换关卡
        else
        {
            _currentLevel = n;
            ResetLevelState();
            SceneManager.LoadScene("Level_"+n.ToString("00"));
            _BGMIndex++;
            AudioManager.Instance.PlayBGM(_BGMIndex);
            UIManager.Instance.ShowHUD();
        }
    
    }
    public void LoadNextLevel()
    {
        _isCompleting = false;
        LoadLevel(_currentLevel+1);
    }
    public void RegisterPlayer(PlayerController player)
    {
        _player = player;
        _lastSavePoint = player.gameObject.transform.position;
        _isDead = false;
        _loading = false;
        _inLevel = true;
        ResetLevelState();
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        EventCenter.Broadcast(GameEvent.Resume);
        //UI
        UIManager.Instance.ShowHUD();
        UIManager.Instance.HidePauseMenu();
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        EventCenter.Broadcast(GameEvent.Pause);
        //UI
        UIManager.Instance.HideHUD();
        UIManager.Instance.ShowPauseMenu();
    }
    public void BackToMainMenu()
    {
        _isCompleting =false;
        Time.timeScale = 1f;
        UIManager.Instance.HidePauseMenu();
        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance.PlayBGM(_MainMenuBGMIndex);
        _inLevel = false;
    }
    public void OnPlayerDeathAnimationFinished()
    {
        if (_player != null) _player.Respawn(_lastSavePoint);
        _isDead = false;
        EventCenter.Broadcast(GameEvent.PlayerRespawned);
        Time.timeScale = 1f;
    }
}
