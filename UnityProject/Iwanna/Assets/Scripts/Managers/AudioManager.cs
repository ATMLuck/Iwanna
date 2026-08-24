using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip[] _bgmList;  //背景音乐列表，用于存储背景音乐文件

    [SerializeField] private bool _loopBGM = true;  //是否循环，如果是则进行循环播放

    public void PlayBGM(int index)
    {
        if(_bgmSource == null || _bgmList == null || _bgmList.Length==0)
        return;
        index=Mathf.Clamp(index)
        //播放音乐
        SetMusicVolume(ProgressManager.Instance.MusicVolume);
    }

    public void SetMusicVolume(float volume)
    {
        //设置音乐音量
        _bgmSource.volume = Mathf.Clamp01(volume);

    }

    public void SetSFXVolume(float volume)
    {
        //设置音效音量
        _sfxSource.volume = Mathf.Clamp01(volume);
    }


    public void PlaySFX(AudioClip clip)
    {
        //播放音效
        SetMusicVolume(ProgressManager.Instance.SFXVolume);
        _sfxSource.PlayOneShot(clip);

    }

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;
        // AudioManager 初始化
    }
}
