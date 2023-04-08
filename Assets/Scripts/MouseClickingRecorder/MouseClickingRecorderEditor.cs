using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(MouseClickingRecorder))]
public class MouseClickingRecorderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawClickingButton();
        DrawDefaultInspector();
    }

    void DrawClickingButton()
    {
        MouseClickingRecorder mouseClickingRecorder = (MouseClickingRecorder)target;
        bool isRecording = mouseClickingRecorder.isRecordingMouseClicks;

        if (isRecording) 
            mouseClickingRecorder.StartRecording();
        else 
            mouseClickingRecorder.StopRecording();

        if (GUILayout.Button(new GUIContent("Click Me", "Click this button to record your mouse clicks."), GUILayout.Height(100)) && isRecording)
            mouseClickingRecorder.AddKeyFrame();
    }
}

#endif