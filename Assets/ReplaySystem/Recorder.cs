using UnityEngine;
using System.Collections.Generic;

public class Recorder : MonoBehaviour
{
    [Header("Prefab To Instantiate")]
    [SerializeField] private GameObject replayObjectPrefab;

    public Queue<ReplayData> recordingQueue { get; private set; }

    private List<Recording> recordings;

    [SerializeField] private Transform respawnLocation;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        recordingQueue = new Queue<ReplayData>();
        recordings = new List<Recording>();
    }

    private void Start()
    {
        GameEventManager.instance.onCountdownEnds += CountdownEnds;
        GameEventManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnDestroy()
    {
        if (GameEventManager.instance == null)
            return;

        GameEventManager.instance.onCountdownEnds -= CountdownEnds;
        GameEventManager.instance.onRestartLevel -= OnRestartLevel;
    }

    private void Update()
    {
        // Play every existing replay clone
        for (int i = 0; i < recordings.Count; i++)
        {
            recordings[i].PlayNextFrame(Time.deltaTime);
        }
    }

    public void RecordReplayFrame(ReplayData data)
    {
        recordingQueue.Enqueue(data);

        // Careful: this prints every frame.
        // Debug.Log("Recorded data: " + data.position);
    }

    private void CountdownEnds()
    {
        GetComponent<CharacterController>().enabled = false;
        player.transform.position = respawnLocation.position;
        GetComponent<CharacterController>().enabled = true;


        CreateReplay();
    }

    private void CreateReplay()
    {
        if (recordingQueue.Count == 0)
        {
            Debug.LogWarning(
                "Cannot create replay: recording queue is empty."
            );

            return;
        }

        // Create a permanent copy of the player's latest recording
        Recording newRecording = new Recording(recordingQueue);
        
        if (recordings.Count < 8)
        {
            newRecording.InstantiateReplayObject(replayObjectPrefab);
            recordings.Add(newRecording);
        }

        // Keep track of it
        foreach (Recording recording in recordings)
        {
            recording.RestartFromBeginning();
        }

        // Start recording the next attempt
        recordingQueue.Clear();

        Debug.Log(
            "Replay created. Total clones: "
            + recordings.Count
        );
    }

    private void OnRestartLevel()
    {
        Reset();
    }

    private void Reset()
    {

        // Destroy every replay clone
        foreach (Recording recording in recordings)
        {
            recording.DestroyReplayObjectIfExists();
        }

        // Remove all recordings
        recordings.Clear();

        // Clear the player's current unfinished recording
        recordingQueue.Clear();



        Debug.Log("Recorder reset.");
    }
}