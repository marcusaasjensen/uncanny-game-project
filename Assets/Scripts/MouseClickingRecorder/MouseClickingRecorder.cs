using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
public class MouseClickingRecorder : MonoBehaviour
{
    [System.Serializable]
    public class Recording
    {
        public string tag;
        public int index = 0;
        public List<double> timeBetweenClicks;
    }

    [Tooltip("Check this box to start recording your mouse clicks. Uncheck if you want to stop recording.")]
    public bool isRecordingMouseClicks = false;

    [Tooltip("Tag of the recording you want to affect when recording your mouse clicks.")]
    public string recordingTag;

    [Tooltip("List of all of your timing recordings. Make sure to create an empty or an existing recording and use its tag to record your timings with your mouse.")]
    public List<Recording> recordingList;

    public static MouseClickingRecorder Instance;

    double timeOffset;
    double timer;

    bool hasStartedRecording;
    bool isFirstClick;

    void Awake() 
    {
        #region Singleton
        if (Instance == null) Instance = this;
        #endregion
    }

    public Recording ContainsRecordingWithTag(string tag)
    {
        foreach(Recording recording in recordingList)
            if(recording.tag == tag) return recording;
        return null;
    }

    public void StartRecording()
    {
        Recording recording = ContainsRecordingWithTag(recordingTag);
        if (recording is null)
        {
            Debug.LogWarning("the recording with tag \"" + recordingTag + "\" does not exist.");
            isRecordingMouseClicks = false;
            return;
        }

        if (hasStartedRecording) return;
        hasStartedRecording = true;
        isFirstClick = true;

        Debug.Log($"<color=#AAFF00>Waiting for the first click before starting recording...</color>");
    }

    public void StopRecording()
    {
        if (hasStartedRecording)
        {
            ContainsRecordingWithTag(recordingTag).timeBetweenClicks.Add(0);
            Debug.Log($"<color=#AAFF00>Recording stopped.</color>"); 
        }
        hasStartedRecording = false;
        timer = 0;
    }

    public void AddKeyFrame()
    {
        List<double> keyFrameList = ContainsRecordingWithTag(recordingTag).timeBetweenClicks;

        if (isFirstClick)
        {
            timeOffset = EditorApplication.timeSinceStartup;
            isFirstClick = false;

            if (keyFrameList.Count > 0 && keyFrameList[^1] == 0)
                keyFrameList.RemoveAt(keyFrameList.Count-1);

            if (keyFrameList.Count == 0)
                keyFrameList.Add(0);

            Debug.Log($"<color=#AAFF00>Recording...</color>");
            return;
        }

        double previousTime = timer;
        timer = EditorApplication.timeSinceStartup - timeOffset;
        double timeBetweenTwoClicks = timer - previousTime;

        keyFrameList.Add(timeBetweenTwoClicks);
    }
}