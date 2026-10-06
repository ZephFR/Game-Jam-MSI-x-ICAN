using System;
using UnityEngine;
using System.Collections.Generic;

public class Recorder : MonoBehaviour
{
    [Header("Prefab To Instantiate")] 
    [SerializeField] private GameObject replayObjectPrefab;
    
    public Queue<ReplayData> recordingQueue { get; private set; }

    private Recording recording;
    
    private bool isDoingReplay = false;

    private void Awake()
    {
        recordingQueue = new Queue<ReplayData>();
    }

    private void Start()
    {
        GameEventManager.instance.onCountdownEnds += CountdownEnds;
        GameEventManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnDestroy()
    {
        GameEventManager.instance.onCountdownEnds -= CountdownEnds;
        GameEventManager.instance.onRestartLevel -= OnRestartLevel;
    }

    private void CountdownEnds()
    {
        StartReplay();
    }

    private void OnRestartLevel()
    {
        Reset();
    }
    
    
    private void Update()
    {
        if (!isDoingReplay)
            return;

        bool hasMoreFrames = recording.PlayNextFrame();
        if (!hasMoreFrames)
        {
            RestartReplay();
        }
    }

    public void RecordReplayFrame(ReplayData data)
    {
        recordingQueue.Enqueue(data);
        Debug.Log("Recorded data : " + data.position);
    }

    private void StartReplay()
    {
        isDoingReplay = true;
        recording = new Recording(recordingQueue);
        recordingQueue.Clear();
        recording.InstantiateReplayObject(replayObjectPrefab);
    }

    private void RestartReplay()
    {
        isDoingReplay = true;
            
        recording.RestartFromBeginning();
    }

    private void Reset()
    {
        isDoingReplay = false;
        recordingQueue.Clear();
        recording.DestroyReplayObjectIfExists();
        recording = null;
    }
}
