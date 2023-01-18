using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RecordingPlayer : MonoBehaviour
{
    [Header("Player Settings")]

    [Space]
    public MouseClickingRecorder mouseClickingRecorder;

    [Tooltip("Tag of the recording you want to play.")]
    public string recordingTagToPlay;

    [Tooltip("Time to wait before the recording is played.")]
    [Min(0)]
    public float timeBeforeStarting;

    public Dictionary<string, MouseClickingRecorder.Recording> recordingDictionary;

    void Start()
    {
        mouseClickingRecorder = MouseClickingRecorder.Instance;

        if (mouseClickingRecorder.ContainsRecordingWithTag(recordingTagToPlay) == null) 
        {
            Debug.LogWarning("The recording \"" + recordingTagToPlay + "\" does not exist.");    
            return; 
        }

        recordingDictionary = new Dictionary<string, MouseClickingRecorder.Recording>();
        foreach (MouseClickingRecorder.Recording recording in mouseClickingRecorder.recordingList)
            recordingDictionary.Add(recording.tag, recording);
    }

    [ContextMenu("Play Recording (In Play Mode)")]
    public void PlayCurrentRecording()
    {
        if (mouseClickingRecorder.ContainsRecordingWithTag(recordingTagToPlay) == null)
        {
            Debug.LogWarning("The recording \"" + recordingTagToPlay + "\" does not exist.");
            return;
        }
        StartCoroutine(PlayRecordingFromStart(recordingTagToPlay));
    }

    public void StopRecording()
    {
        if (mouseClickingRecorder.ContainsRecordingWithTag(recordingTagToPlay) == null)
        {
            Debug.LogWarning("The recording \"" + recordingTagToPlay + "\" does not exist.");
            return;
        }
        StopCoroutine(PlayRecordingFromStart(recordingTagToPlay));
    }

    IEnumerator PlayRecordingFromStart(string tag)
    {
        yield return new WaitForSeconds(timeBeforeStarting);
        MouseClickingRecorder.Recording recording = recordingDictionary[tag];
        recording.index = 0;

        for (int i = 0; i < recording.timeBetweenClicks.Count; i++)
        {
            recording.index=i;
            yield return new WaitForSeconds((float) recording.timeBetweenClicks[i]);
        }
    }
}