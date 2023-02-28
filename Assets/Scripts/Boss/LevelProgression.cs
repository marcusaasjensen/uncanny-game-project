using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelProgression : MonoBehaviour
{
    public List<RecordingPlayer> rhytmicSet;
    public PlayableDirector timeLine;
    public ProgressBarController progressBar;
    public static bool IsLevelCompleted = false;
    void Start() => StartLevelBoss();
    void Update()
    {
        if (!progressBar) return;
        IsLevelCompleted = progressBar.HasEnded || IsLevelCompleted;
    }
    //A rhytmic set is a set of rhythms that are used in a precised scenario. It can be music rhythm, events when projectiles have to follow a certain rhythm.
    void StartRhythmicSet()
    {
        foreach (RecordingPlayer recording in rhytmicSet)
            recording.PlayCurrentRecording();
    }

    void StopRhythmicSet()
    {
        foreach (RecordingPlayer recording in rhytmicSet)
            recording.StopRecording();
    }

    public void StartLevelBoss()
    {
        timeLine.Play();
        StartRhythmicSet();
    }

    public void StopLevelBoss()
    {
        timeLine.Stop();
        StopRhythmicSet();
        SoundManager.Instance.StopMusic();
    }
}
