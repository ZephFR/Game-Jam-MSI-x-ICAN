using UnityEngine;
using System.Collections.Generic;

public class Recorder : MonoBehaviour
{
    [Header("Prefab To Instantiate")]
    [SerializeField] private GameObject replayObjectPrefab;

    [Header("Recording Settings")]
    [SerializeField] private float recordingDuration = 30f;

    public Queue<ReplayData> recordingQueue { get; private set; }

    
    private List<Recording> recordings;

    
    private float recordingTimer;

    private void Awake()
    {
        recordingQueue = new Queue<ReplayData>();

        recordings = new List<Recording>();

        recordingTimer = 0f;
    }

    private void Start()
    {
        GameEventManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnDestroy()
    {
        if (GameEventManager.instance != null)
        {
            GameEventManager.instance.onRestartLevel -= OnRestartLevel;
        }
    }

    private void Update()
    {

        recordingTimer += Time.deltaTime;

       
        if (recordingTimer >= recordingDuration)
        {
            CreateReplay();

            recordingTimer = 0f;
        }

        for (int i = 0; i < recordings.Count; i++)
        {
            bool hasMoreFrames = recordings[i].PlayNextFrame();

            
            if (!hasMoreFrames)
            {
                
                recordings[i].RestartFromBeginning();
            }
        }
    }

    public void RecordReplayFrame(ReplayData data)
    {
        recordingQueue.Enqueue(data);

        Debug.Log("Recorded data : " + data.position);
    }

    private void CreateReplay()
    {
        if (recordingQueue.Count == 0)
        {
            Debug.LogWarning("Cannot create replay: recording is empty.");
            return;
        }

        Recording newRecording = new Recording(recordingQueue);

        newRecording.InstantiateReplayObject(replayObjectPrefab);
        

        recordings.Add(newRecording);

        recordingQueue.Clear();

        Debug.Log(
            "New replay created. Total clones: "
            + recordings.Count
        );
    }

    private void OnRestartLevel()
    {
        Reset();
    }

    private void Reset()
    {
        
        recordingTimer = 0f;
        recordingQueue.Clear();

        foreach (Recording recording in recordings)
        {
            recording.DestroyReplayObjectIfExists();
        }
        
        recordings.Clear();
        Debug.Log("Recorder reset.");
    }
}