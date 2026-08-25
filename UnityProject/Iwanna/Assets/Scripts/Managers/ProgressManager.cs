using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;



[System.Serializable]
public class ProgressData
{
    public int totalLevels;
    public float musicVolume;
    public float sfxVolume;
    public int bgmIndex;
    public int[] unlockedLevels;

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
        if (level < 1 || level > _data.totalLevels)
            return false;
        if (_data.unlockedLevels == null)
            return false;
        return Array.IndexOf(_data.unlockedLevels, level) >= 0;
    }
    public void UnlockLevel(int level)
    {
        //判断IsLevelUnlocked，否则不解锁关卡
        //实施关卡解锁,添加目标编号到unlockedLevels
        EnsureLoaded();
        if (level < 1 || level > _data.totalLevels)
        {
            Debug.LogWarning($"无法解锁非法关卡：{level}");
            return;
        }

        if (IsLevelUnlocked(level))
            return;

        Array.Resize(
            ref _data.unlockedLevels,
            _data.unlockedLevels.Length + 1
        );

        _data.unlockedLevels[
            _data.unlockedLevels.Length - 1
        ] = level;

        Array.Sort(_data.unlockedLevels);
        Save();
    }
    public void Load()
    {
        // 加载游戏进度，处理所有文件异常情况：
        // 文件不存在、内容为空、JSON 损坏、反序列化失败等
        // 任何异常均恢复默认配置并输出警告

        if (!File.Exists(_filePath))
        {
            Debug.LogWarning("Json file not found. Using default configuration.");
            _data = CreateDefaultData();
            _isLoaded = true;
            Save();
            return;
        }

        try
        {
            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("Json file is empty or whitespace-only.");

            ProgressData loadedData;
            try
            {
                loadedData = JsonUtility.FromJson<ProgressData>(json);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("config.json 格式不正确", e);
            }

            if (loadedData == null)
                throw new Exception("Loaded progress data is null.");

            _data = loadedData;

            bool repaired = ValidateAndRepairData();

            _isLoaded = true;

            if (repaired)
                Save();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to load progress data: {e.Message}. Using default configuration.");
            _data = CreateDefaultData();
            _isLoaded = true;
            Save();
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
            newVolume = NormalizeVolume(newVolume);
            if (Mathf.Approximately(newVolume, _data.musicVolume))
                return;
            _data.musicVolume = newVolume;
            Save();
        }
    }
    private float NormalizeVolume(float volume)
    {
        // 规范化音量值，确保其在0到1之间
        if (float.IsNaN(volume) || float.IsInfinity(volume))
            return DefaultVolume;
        return Mathf.Clamp01(volume);
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
            newVolume = NormalizeVolume(newVolume);
            if (Mathf.Abs(newVolume - _data.sfxVolume) <= 0.001f)
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
            if (newIndex == _data.bgmIndex)
                return;
            _data.bgmIndex = newIndex;
            Save();
        }
    }

    private ProgressData CreateDefaultData()
    {
        return new ProgressData
        {
            totalLevels = DefaultTotalLevels,
            musicVolume = DefaultVolume,
            sfxVolume = DefaultVolume,
            bgmIndex = DefaultBGMIndex,
            unlockedLevels = new int[] { 1 }
        };
    }
    private bool ValidateAndRepairData()
    {
        bool repaired = false;

        if (_data.totalLevels != DefaultTotalLevels)
        {
            _data.totalLevels = DefaultTotalLevels;
            repaired = true;
        }

        List<int> validLevels = new List<int> { 1 };
        if (_data.unlockedLevels != null)
        {
            foreach (int level in _data.unlockedLevels)
            {
                if (level < 1 || level > _data.totalLevels)
                    continue;
                if (!validLevels.Contains(level))
                    validLevels.Add(level);
            }
        }
        validLevels.Sort();
        int[] repairedLevels = validLevels.ToArray();

        if (_data.unlockedLevels == null || repairedLevels.Length != _data.unlockedLevels.Length)
        {
            _data.unlockedLevels = repairedLevels;
            repaired = true;
        }
        else
        {
            for (int i = 0; i < repairedLevels.Length; i++)
            {
                if (repairedLevels[i] != _data.unlockedLevels[i])
                {
                    _data.unlockedLevels = repairedLevels;
                    repaired = true;
                    break;
                }
            }
        }
        float repairedMusic = NormalizeVolume(_data.musicVolume);
        if (!Mathf.Approximately(repairedMusic, _data.musicVolume))
        {
            _data.musicVolume = repairedMusic;
            repaired = true;
        }

        float repairedSfx = NormalizeVolume(_data.sfxVolume);
        if (!Mathf.Approximately(repairedSfx, _data.sfxVolume))
        {
            _data.sfxVolume = repairedSfx;
            repaired = true;
        }

        if (_data.bgmIndex < 0)
        {
            _data.bgmIndex = DefaultBGMIndex;
            repaired = true;
        }

        return repaired;
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
        try
        {
            if (!_isLoaded || _data == null)
                return;
            File.WriteAllText(_filePath, JsonUtility.ToJson(_data, true));
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving progress data: " + e.Message);
        }

        // 实现保存数据的逻辑，例如将数据写入Json文件
    }
}