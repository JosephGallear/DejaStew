using System.Collections.Generic;
using UnityEngine;

public class CloneMovement : MonoBehaviour
{
    [SerializeField] private GameObject particleGameObject;

    private List<TransformFrame> transformFrames = new List<TransformFrame>();
    private float loopDuration;
    private float timeElapsed;

    private int currentFrameIndex;

    private Vector3 startPosition;

    public void Initialise(List<TransformFrame> recordedTransformFrames, float duration, Vector3 startPos)
    {
        transformFrames = new List<TransformFrame>(recordedTransformFrames);
        loopDuration = duration;
        startPosition = startPos;

        timeElapsed = 0f;
        currentFrameIndex = 0;

        if (transformFrames.Count > 0)
            transform.position = transformFrames[0].position;
        else
            transform.position = startPosition;
    }

    private void Update()
    {
        if (transformFrames == null || transformFrames.Count == 0) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed > loopDuration)
        {
            timeElapsed = 0f;
            currentFrameIndex = 0;
            particleGameObject.SetActive(false);
            transform.position = startPosition;
            particleGameObject.SetActive(true);
            if (transformFrames.Count > 0)
                transform.rotation = transformFrames[0].rotation;
            return;
        }

        while (currentFrameIndex < transformFrames.Count - 1 && transformFrames[currentFrameIndex + 1].timestamp <= timeElapsed)
        {
            currentFrameIndex++;
        }

        TransformFrame currentFrame = transformFrames[currentFrameIndex];
        TransformFrame nextFrame = transformFrames[(currentFrameIndex + 1) % transformFrames.Count];

        float frameDuration = nextFrame.timestamp - currentFrame.timestamp;

        if (frameDuration < 0)
        {
            frameDuration += loopDuration;
        }

        float t = frameDuration > 0f ? (timeElapsed - currentFrame.timestamp) / frameDuration : 0f;

        transform.position = Vector3.Lerp(currentFrame.position, nextFrame.position, t);
        transform.rotation = Quaternion.Slerp(currentFrame.rotation, nextFrame.rotation, t);
    }
}