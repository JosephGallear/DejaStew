using System;
using System.Collections.Generic;
using UnityEngine;

public class CloneInput : MonoBehaviour, IHasInput
{
    [SerializeField] private Player player;

    public event EventHandler OnInteractAction;
    public event EventHandler OnUseAction;

    private List<InputFrame> inputFrames = new List<InputFrame>();
    private float loopDuration;
    private float timeElapsed;
    private float lastPlaybackTimer;

    private int currentFrameIndex;

    private Vector2 currentMovement;

    public void Initialise(List<InputFrame> recordedFrames, float duration)
    {
        inputFrames = new List<InputFrame>(recordedFrames);
        loopDuration = duration;

        timeElapsed = 0f;
        lastPlaybackTimer = 0f;
        currentFrameIndex = 0;
    }

    private void Update()
    {
        if (inputFrames == null || inputFrames.Count == 0) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed > loopDuration)
        {
            timeElapsed = 0f;
            lastPlaybackTimer = 0f;
            currentFrameIndex = 0;
        }

        float totalMovementX = 0f;
        float totalMovementY = 0f;

        while (currentFrameIndex < inputFrames.Count && inputFrames[currentFrameIndex].timestamp <= timeElapsed)
        {
            InputFrame frame = inputFrames[currentFrameIndex];

            if (frame.timestamp > lastPlaybackTimer)
            {
                totalMovementX += frame.movement.x;
                totalMovementY += frame.movement.y;

                if (frame.interactPressed)
                {
                    OnInteractAction?.Invoke(this, EventArgs.Empty);
                }

                if (frame.usePressed)
                {
                    OnUseAction?.Invoke(this, EventArgs.Empty);
                }
            }

            currentFrameIndex++;
        }

        currentMovement = new Vector2(totalMovementX, totalMovementY).normalized;

        player.SetIsWalking(currentMovement != Vector2.zero);

        lastPlaybackTimer = timeElapsed;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return currentMovement;
    }
}
