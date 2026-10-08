using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Recorder : MonoBehaviour
{
    [Header("Prefab To Instantiate")]
    [SerializeField] private GameObject replayObjectPrefab;

    public Queue<ReplayData> recordingQueue { get; private set; }

    private List<Recording> recordings;

    [SerializeField] private Transform respawnLocation;
    [SerializeField] private GameObject player;

    private IInteractible interactionThisFrame;
    public float clonesRemain;

    [SerializeField] private int microphoneFrequency = 44100;

    private string microphoneDevice;
    private AudioClip currentMicRecording;

    private void Awake()
    {
        recordingQueue = new Queue<ReplayData>();
        recordings = new List<Recording>();

        StartMicrophoneRecording();
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

        clonesRemain = 8 - recordings.Count;
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

        AudioClip micClip = StopMicrophoneRecording();
        // Create a permanent copy of the player's latest recording
        Recording newRecording = new Recording(recordingQueue, micClip);

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

        StartMicrophoneRecording();
    }


    private void StartMicrophoneRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone detected.");
            return;
        }

        microphoneDevice = Microphone.devices[0];
        Debug.Log("Microphone : " + microphoneDevice);

        currentMicRecording = Microphone.Start(microphoneDevice, false, 30, microphoneFrequency);
    }
    private AudioClip StopMicrophoneRecording()
    {
        if (string.IsNullOrEmpty(microphoneDevice) || !Microphone.IsRecording(microphoneDevice))
            return null;

        int samplePosition = Microphone.GetPosition(microphoneDevice);
        Microphone.End(microphoneDevice);

        if (currentMicRecording == null || samplePosition <= 0)
            return null;

        int channels = currentMicRecording.channels;

        float[] samples = new float[samplePosition * channels];

        currentMicRecording.GetData(samples, 0);

        AudioClip trimmedClip = AudioClip.Create("ReplayMicRecording", samplePosition, channels, microphoneFrequency, false);
        trimmedClip.SetData(samples, 0);
        currentMicRecording = null;
        return trimmedClip;
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


        interactionThisFrame = null;
        Debug.Log("Recorder reset.");
    }
    
    public void RecordInteraction(IInteractible interactible)
    {
        interactionThisFrame = interactible;
    }

    public IInteractible ConsumeInteraction()
    {
        IInteractible interaction = interactionThisFrame;

        interactionThisFrame = null;

        return interaction;
    }
}