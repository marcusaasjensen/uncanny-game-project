using System.Collections.Generic;
using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    public AudioClip music; //to add in an audio library
    public List<RecordingPlayer> rhytmicSet;
    public LevelBoss levelBoss;

    void Start() => StartLevelBoss();

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
        levelBoss.StartActionSequence();
        StartRhythmicSet();
        SoundManager.Instance.PlayMusic(music);
    }

    public void StopLevelBoss()
    {
        levelBoss.StopActionSequence();
        StopRhythmicSet();
        SoundManager.Instance.StopMusic();
    }
}
