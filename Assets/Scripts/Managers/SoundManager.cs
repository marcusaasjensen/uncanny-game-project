using UnityEngine;
using System.Collections.Generic;
using Unity.Profiling;
using System.Collections;

[DisallowMultipleComponent]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource _musicSource, _effectsSource, _testSource;

    [SerializeField] static float musicVolume = 1f, sfxVolume = 1f;
    [SerializeField] AudioClip _testSound;

    public float MusicVolume { get { return musicVolume; } }
    public float SFXVolume { get { return sfxVolume; } }

    void Awake()
    {
        _musicSource.volume = musicVolume; 
        _effectsSource.volume = sfxVolume;

        _musicSource.Stop();
        _effectsSource.Stop();

        #region Singleton
        if (Instance == null)
            Instance = this;

        #endregion
    }

    void Update() => OnPause();

    void OnPause()
    {
        if (PauseMenu.IsGamePaused)
        {
            _musicSource.Pause();
            _effectsSource.Pause();
        }
        else if (!_musicSource.isPlaying)
        {
            _musicSource.UnPause();
            _effectsSource.UnPause();
        }
    }

    public void LoopMusic(AudioClip clip)
    {
        if (clip == null) return;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        _effectsSource.PlayOneShot(clip);
    }

    public void TestSoundWithVolume(float value)
    {
        if(_testSound == null || _testSource.isPlaying) return;
        _testSource.PlayOneShot(_testSound, value);
    }

    int _indexTMP = -1;

    public void PlayRandomSound(List<AudioClip> clipList, bool neverPlayInARow)
    {
        if (clipList.Count == 0 || clipList == null) return;

        int randomIndex;

        if(neverPlayInARow && clipList.Count > 1)
            do
                randomIndex = Random.Range(0, clipList.Count - 1);
            while (randomIndex == _indexTMP);
        else
            randomIndex = Random.Range(0, clipList.Count - 1); ;

        _indexTMP = randomIndex;
        PlaySound(clipList[randomIndex]);
    }

    public void PlayAllSound(List<AudioClip> clipList)
    {
        if (clipList == null) return;
        foreach(AudioClip clip in clipList)
            PlaySound(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        _musicSource.PlayOneShot(clip);
    }

    public void StopMusic() => _musicSource.Stop();

    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        _musicSource.volume = volume;
    }

    public void ChangeSFXVolume(float volume) 
    { 
        sfxVolume = volume;
        _effectsSource.volume = volume;
    }

    [ContextMenu("Toggle Effects")]
    public void ToggleEffects()
    {
        _effectsSource.mute = !_effectsSource.mute;
    }

    [ContextMenu("Toggle Music")]
    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }
}
