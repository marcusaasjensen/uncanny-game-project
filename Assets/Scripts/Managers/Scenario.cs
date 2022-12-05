using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Scenario : MonoBehaviour
{
    public Animator anim;
    public AudioClip music; //to add in an audio library
    public List<RecordingPlayer> rhytmicSet;
    public ProjectileController projectilePrefab;

    void Start() => StartLevelBoss();

    void StartLevelBoss()
    {
        StartRhythmicSet();
        SoundManager.Instance.PlayMusic(music);

    }

    //A rhytmic set is a set of rhythms that are used in a precised scenario. It can be music rhythm, events when projectiles have to follow a certain rhythm.
    void StartRhythmicSet()
    {
        foreach (RecordingPlayer recording in rhytmicSet)
            recording.PlayCurrentRecording();
    }


}
