using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip[] _bgmList;  //背景音乐列表，用于存储背景音乐文件
    public int BgmListLength => _bgmList != null ? _bgmList.Length : 0;

    [SerializeField] private bool _loopBGM = true;  //是否循环，如果是则进行循环播放
    [SerializeField] private AudioClip _playerDeathClip;  //玩家死亡音效
    [SerializeField] private AudioClip _shootClip;  //射击音效
    [SerializeField] private AudioClip _jumpClip;  //跳跃音效
    [SerializeField] public int _bgmListLength;

    public void PlayBGM(int index)
    {
        if (_bgmSource == null || _bgmList == null || _bgmList.Length == 0)
            return;
        if (index < 0 || index >= _bgmList.Length)
            return;
        //播放背景音乐
        _bgmSource.loop = _loopBGM;
            _bgmSource.clip = _bgmList[index];
            _bgmSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        if (_bgmSource == null)
        {
            Debug.LogWarning("AudioManager: BGMSource is not assigned.");
            return;
        }
        //设置音乐音量
        _bgmSource.volume = Mathf.Clamp01(volume);

    }

    public void SetSFXVolume(float volume)
    {
        if (_sfxSource == null)
        {
            Debug.LogWarning("AudioManager: SFXSource is not assigned.");
            return;
        }
        //设置音效音量
        _sfxSource.volume = Mathf.Clamp01(volume);
    }


    public void PlaySFX(SFXType type)
    {
        if (_sfxSource == null)
            return;

        AudioClip clip = GetAudioClip(type);
        if (clip == null)
        {
            Debug.LogWarning("没有配置音效： " + type);
            return;
        }
        _sfxSource.PlayOneShot(clip);
    }

    private AudioClip GetAudioClip(SFXType type)
    {
        switch (type)
        {
            case SFXType.PlayerDeath:
                return _playerDeathClip;
            case SFXType.Jump:
                return _jumpClip;
            case SFXType.Shoot:
                return _shootClip;
            default:
                return null;
        }

    }
    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;
        // AudioManager 初始化
    }
}
