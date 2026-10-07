using UnityEngine;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;

public class Recording
{
    public ReplayObject ReplayObject { get; private set; }

    private Queue<ReplayData> originalQueue;
    private Queue<ReplayData> replayQueue;

    private float replayTimer;
    private float maxReplayDuration = 30f;

    public Recording(Queue<ReplayData> recordingQueue)
    {
        // Permanent copy of the recording
        originalQueue = new Queue<ReplayData>(recordingQueue);

        // Queue that will actually be consumed while replaying
        replayQueue = new Queue<ReplayData>(originalQueue);

        replayTimer = 0;
    }

    public void RestartFromBeginning()
    {
        replayQueue = new Queue<ReplayData>(originalQueue);
        replayTimer = 0;

    }

    public void PlayNextFrame(float deltaTime)
    {
        if (ReplayObject == null)
        {
            Debug.LogError("ReplayObject is null.");
            return;
        }

        replayTimer += deltaTime;
        if (replayTimer >= maxReplayDuration)
        {
            RestartFromBeginning();
            return;
        }

        // Safety check
        if (replayQueue.Count == 0)
        {
            return;
        }

        ReplayData data = replayQueue.Dequeue();

        ReplayObject.SetDataForFrame(data);
    }

    public void InstantiateReplayObject(GameObject replayObjectPrefab)
    {
        if (replayQueue.Count == 0)
        {
            Debug.LogWarning("Cannot instantiate ReplayObject: recording is empty.");
            return;
        }

        ReplayData startingData = replayQueue.Peek();
        GameObject replayObject = Object.Instantiate(replayObjectPrefab, startingData.position, Quaternion.identity);
        ReplayObject = replayObject.GetComponent<ReplayObject>();
        if (ReplayObject == null)
        {
            Debug.LogError(
                "The replay prefab does not have a ReplayObject component."
            );
        }
    }

    public void DestroyReplayObjectIfExists()
    {
        if (ReplayObject == null)
            return;

        Object.Destroy(ReplayObject.gameObject);

        ReplayObject = null;
    }
}