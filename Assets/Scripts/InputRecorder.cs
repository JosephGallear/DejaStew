using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InputFrame
{
    public float timestamp;
    public Vector2 movement;
    public bool interactPressed;
    public bool usePressed;
}

[System.Serializable]
public struct TransformFrame
{
    public float timestamp;
    public Vector3 position;
    public Quaternion rotation;
}

public class InputRecorder : MonoBehaviour
{
    [SerializeField] private CloneRecordingUI cloneRecordingUI;

    [SerializeField] private float maxRecordingDuration = 5f;

    private List<TransformFrame> recordedTransformFrames = new List<TransformFrame>();
    private List<InputFrame> recordedInputFrames = new List<InputFrame>();

    private float recordingStartTime;
    private bool isRecording;

    private Vector3 recordedStartPosition;

    private void Start()
    {
        GameInput.Instance.OnRecordAction += GameInput_OnRecordAction;
    }

    private void GameInput_OnRecordAction(object sender, EventArgs e)
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    public void StartRecording()
    {
        if (CloneManager.Instance.IsCloneLimitReached()) return;

        cloneRecordingUI.Show();

        recordedStartPosition = transform.position;

        recordedTransformFrames.Clear();
        recordedInputFrames.Clear();

        recordingStartTime = Time.time;
        isRecording = true;

        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnUseAction += GameInput_OnUseAction;
    }

    public void StopRecording()
    {
        isRecording = false;

        cloneRecordingUI.Hide();

        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
        GameInput.Instance.OnUseAction -= GameInput_OnUseAction;

        float recordingDuration = recordedTransformFrames.Count > 0 ? recordedTransformFrames[^1].timestamp : 0f;

        CloneManager.Instance.SpawnClone(recordedStartPosition, recordedTransformFrames, recordedInputFrames, recordingDuration);

        recordedTransformFrames.Clear();
        recordedInputFrames.Clear();
        recordedStartPosition = Vector3.zero;
    }

    private void GameInput_OnUseAction(object sender, EventArgs e)
    {
        RecordUse();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        RecordInteract();
    }

    private void Update()
    {
        if (!isRecording) return;

        float timeStamp = Time.time - recordingStartTime;

        if (timeStamp > maxRecordingDuration)
        {
            StopRecording();
            return;
        }

        recordedTransformFrames.Add(new TransformFrame
        {
            timestamp = timeStamp,
            position = transform.position,
            rotation = transform.rotation
        });

        recordedInputFrames.Add(new InputFrame
        {
            timestamp = timeStamp,
            movement = GameInput.Instance.GetMovementVectorNormalized(),
            interactPressed = false,
            usePressed = false
        });
    }

    public void RecordInteract()
    {
        if (!isRecording || recordedInputFrames.Count == 0) return;

        InputFrame frame = recordedInputFrames[^1];
        frame.interactPressed = true;
        recordedInputFrames[^1] = frame;
    }

    public void RecordUse()
    {
        if (!isRecording || recordedInputFrames.Count == 0) return;

        InputFrame frame = recordedInputFrames[^1];
        frame.usePressed = true;
        recordedInputFrames[^1] = frame;
    }
}
