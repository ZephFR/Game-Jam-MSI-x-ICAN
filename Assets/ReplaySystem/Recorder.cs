using UnityEngine;
using System.Collections.Generic;

public class Recorder : MonoBehaviour
{
    public Queue<ReplayData> recordingQueue { get; private set; }

    private void Awake()
    {
        recordingQueue = new Queue<ReplayData>();
    }

    public void RecordReplayFrame(ReplayData data)
    {
        recordingQueue.Enqueue(data);
        Debug.Log("Recorded data : " + data.position);
    }
}
