using UnityEngine;
[System.Serializable]
public class ProgressData
{
    public int totalLevels;
    public float musicVolume;
    public float sfxVolume;
    public int bgmIndex;
}


public class ProgressManager : Singleton<ProgressManager>
{
    private const int DefaultTotalLevels = 3;
    private const float DefaultVolume = 0.8f;
    private const int DefaultBGMIndex = 0;

    private ProgressData _data;
    private string _filePath;
    private bool _isLoaded;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            return;

        _filePath = Path.Combine(Application.persistentDataPath, "config.json");
    }

    public bool IsLevelUnlocked(int level)
    {
        // 检测关卡是否非法，否则直接返回false
        // 检查unlockedLevels 是否包含目标编号
        EnsureLoaded();
        if (level < 0 || level >= _data.totalLevels)
            return false;
        return Array.IndexOf(_data.unlockedLevels, level) >= 0;
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
        if (!_isLoaded)
        {
            return;
        }
    }

    public int TotalLevels
    {
        get
        {
            EnsureLoaded();
            return _data.totalLevels;
        }
    }
    public float MusicVolume
    {
        get
        {
            EnsureLoaded();
            return _data.musicVolume;
        }
        set
        {
            EnsureLoaded();
            float newVolume = Mathf.Clamp(value, 0f, 1f);
            if (Mathf.Approximately(newVolume, _data.musicVolume))
                return;
            _data.musicVolume = newVolume;
            Save();
        }
    }
    public float NormalizeVolume(float volume)
    {
        // 规范化音量值，确保其在0到1之间
        if(float.IsNaN(volume)||float.IsInfinity(volume))
            return DefaultVolume;
        return MathF.Clamp01(volume);
    }
    public float SFXVolume
    {
        get
        {
            EnsureLoaded();
            return _data.sfxVolume;
        }
        set
        {
            EnsureLoaded();
            float newVolume = Mathf.Clamp(value, 0f, 1f);
            if (Mathf.Abs(newVolume - _data.sfxVolume) > 0.01f)
                return;
            _data.sfxVolume = newVolume;
            Save();
        }
    }
    public int BgmIndex
    {
        get
        {
            EnsureLoaded();
            return _data.bgmIndex;
        }
        set
        {
            EnsureLoaded();
            int newIndex = Mathf.Max(0, value);
            _data.bgmIndex = newIndex;
            if (newIndex != _data.bgmIndex)
                return;
            Save();
        }
    }

    private ProgressData CreateDefaultData()
    {
        return _defaultData;
    }
    private bool ValidateAndRepairData()
    {

        return true;

    }
    private void EnsureLoaded()
    {
        // 确保进度数据已加载，如果未加载则调用Load方法加载数据
        if (!_isLoaded)
            Load();
    }
    private void Save()
    {
        // 保存进度数据到文件或数据库
        // 数据没有实际变化，不保存
        // 不在Update中调用

        if (!_isLoaded || _data == null)
            return;
        // 实现保存数据的逻辑，例如将数据写入Json文件
    }
}
