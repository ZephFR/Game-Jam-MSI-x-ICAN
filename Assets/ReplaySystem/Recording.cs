using UnityEngine;
using System.Collections.Generic;

public class Recording
{
    // Liste statique de tous les enregistrements actifs dans la scène
    private static List<Recording> activeRecordings = new List<Recording>();

    public ReplayObject ReplayObject { get; private set; }

    private Queue<ReplayData> originalQueue;
    private Queue<ReplayData> replayQueue;

    private float replayTimer;
    private float maxReplayDuration = 30f;

    private AudioClip recordedAudio;
    private AudioSource replayAudioSource;
    private float replayPitch;

    public Recording(Queue<ReplayData> recordingQueue, AudioClip audioClip)
    {
        // Permanent copy of the recording
        originalQueue = new Queue<ReplayData>(recordingQueue);

        // Queue that will actually be consumed while replaying
        replayQueue = new Queue<ReplayData>(originalQueue);
        
        recordedAudio = audioClip;
        replayTimer = 0;

        replayPitch = Random.Range(0.7f, 1.25f);
    }

    public void RestartFromBeginning()
    {
        replayQueue = new Queue<ReplayData>(originalQueue);
        replayTimer = 0;

        if (ReplayObject != null && replayQueue.Count > 0)
        {
            ReplayData startingData = replayQueue.Peek();
            ReplayObject.SetDataForFrame(startingData);
        }

        if (replayAudioSource != null && recordedAudio != null)
        {
            replayAudioSource.Stop();
            replayAudioSource.clip = recordedAudio;
            replayAudioSource.Play();
        }
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
        replayAudioSource = replayObject.GetComponent<AudioSource>();
        
        if (ReplayObject == null)
        {
            Debug.LogError("The replay prefab does not have a ReplayObject component.");
        }

        if (replayAudioSource != null && recordedAudio != null)
        {
            replayAudioSource.pitch = replayPitch;
            replayAudioSource.clip = recordedAudio;
            replayAudioSource.Play();
        }

        // Ajout de cet enregistrement et mise à jour globale des volumes
        if (!activeRecordings.Contains(this))
        {
            activeRecordings.Add(this);
        }
        UpdateAllVolumes();
    }

    public void DestroyReplayObjectIfExists()
    {
        if (ReplayObject == null)
            return;

        Object.Destroy(ReplayObject.gameObject);
        ReplayObject = null;

        // Retrait de la liste et mise à jour des volumes restants
        activeRecordings.Remove(this);
        UpdateAllVolumes();
    }

    /// <summary>
    /// Recalcule le volume de chaque clone selon son ordre d'apparition.
    /// Dernier clone = 100%, avant-dernier = 50%, etc.
    /// </summary>
    private static void UpdateAllVolumes()
    {
        int count = activeRecordings.Count;
        for (int i = 0; i < count; i++)
        {
            Recording rec = activeRecordings[i];
            if (rec.replayAudioSource != null)
            {
                // Nombre de cran(s) par rapport au plus récent
                int stepsBack = (count - 1) - i;
                
                // Volume = 1 / (2 ^ stepsBack)
                rec.replayAudioSource.volume = Mathf.Pow(0.5f, stepsBack);
            }
        }
    }
}