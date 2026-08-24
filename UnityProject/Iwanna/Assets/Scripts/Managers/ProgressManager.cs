using UnityEngine;
[System.Serializable]
public class ProgressData
{
    public int totalLevels;
    public int[] unlockedLevels;
    public float musicVolume;
    public float sfxVolume;
    public int bgmIndex;
}


public class ProgressManager : Singleton<ProgressManager>
{
    private const int DefaultTotalLevels = 3;
    private const float DefaultVolume = 0.8f;
    private const int DefaultBGMIndex = 0;
    public int[] unlockedLevels = new int[DefaultTotalLevels]{1, 0, 0};
    private ProgressData _data;
    private string _filePath;
    private bool _isLoaded;

    private ProgressData _defaultData = new ProgressData()
    {
        totalLevels = DefaultTotalLevels,
        unlockedLevels = new int[]{1},
        musicVolume = DefaultVolume,
        sfxVolume = DefaultVolume,
        bgmIndex = DefaultBGMIndex
    };
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            return;
    }

    public bool IsLevelUnlocked(int level)
    {
        // 检测关卡是否非法，否则直接返回false
        // 检查unlockedLevels 是否包含目标编号
        return true;
    }
    public void UnlockLevel(int level)
    {
        //判断IsLevelUnlocked，否则不解锁关卡
        //实施关卡解锁，设
    }
    public void Load()
    {
        // 加载游戏进度，需要处理如下Json文件异常情况
        //文件是否为空，JSON文件是否损坏，反序列结果为null等
        //如果Json文件异常，则回复默认配置，并推送警告
    }

    public int TotalLevels {     
        get{
        EnsureLoaded();
        return _data.totalLevels;
        }
     }
    public float MusicVolume {
        get
        {
            EnsureLoaded();
            return _data.musicVolume;
        } set
        {
            EnsureLoaded();
            float newVolume = Mathf.Clamp(value, 0f, 1f);
            if(Mathf.Approximately(newVolume, _data.musicVolume))
                return;
            _data.musicVolume = newVolume;
            Save();
        }}
    public float SFXVolume {
        get
        {
            EnsureLoaded();
            return _data.sfxVolume;
        } set
        {
            EnsureLoaded();
            float newVolume = Mathf.Clamp(value, 0f, 1f);
            if(Mathf.Abs(newVolume - _data.sfxVolume) > 0.01f)
                return;
            _data.sfxVolume = newVolume;
            Save();
        }}
    public int BgmIndex {
        get
        {
            EnsureLoaded();
            return _data.bgmIndex;
        } set
        {
            EnsureLoaded();
            _data.bgmIndex = value;
            Save();
        }}

    private ProgressData CreateDefaultData() {      
        return _defaultData; 
        }
    private bool ValidateAndRepairData() { 
        
        return true; 
        
        }
    private void EnsureLoaded() { }
    private void Save()
    {
        // 保存进度数据到文件或数据库
        //数据没有实际变化，不保存
        //不在Update中调用
    }
}
