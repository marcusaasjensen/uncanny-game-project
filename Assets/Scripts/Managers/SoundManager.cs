using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource _musicSource, _effectsSource;
    void Awake()
    {
        #region Singleton
        if (Instance == null) 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        #endregion
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        _effectsSource.PlayOneShot(clip);
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

    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
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
