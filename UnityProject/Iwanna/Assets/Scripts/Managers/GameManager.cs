using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
//==============================生命周期函数=============================================
    protected override void Awake()
    {
        base.Awake();
        EventCenter.Subscribe(GameEvent.PlayerDeath, OnPlayerDeath);
        EventCenter.Subscribe(GameEvent.SavePointReached, OnSavePoint);
        EventCenter.Subscribe(GameEvent.LevelComplete,OnLevelComplete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
//===============================字段===================================================
    PlayerController _player;
    int _currentLevel = 1;//当前关卡号
    float _elapsedTime = 0;//本关累计时长（秒)
    int _deathCount = 0;//死亡次数
    Vector3 _lastSavePoint = Vector3.zero;//重生位置

    // ---- 属性（public 只读） ----
    public float ElapsedTime
    {
        get{return _elapsedTime;}
    }
    public int DeathCount
    {
        get{return _deathCount;}
    }
//===============================功能实现===============================================
    
    void OnPlayerDeath(object arg)
    {
        _deathCount++;
        
    }
    void OnSavePoint(object arg)
    {
        
    }
    void OnLevelComplete(object arg)
    {
        
    }
    void ResetLevelState()//清空计时/死亡次数
    {
        
    }


//===============================接口===================================================
    public void LoadLevel(int n)
    {
    
    }
    public void LoadNextLevel()
    {
    
    }
    public void RegisterPlayer(PlayerController player)
    {
        
    }
    public void RestartLevel()
    {
        
    }
    public void ResumeGame()
    {
        
    }
    public void PauseGame()
    {
        
    }
    public void BackToMainMenu()
    {
        
    }
    public void OnPlayerDeathAnimationFinished()
    {
        
    }
}
