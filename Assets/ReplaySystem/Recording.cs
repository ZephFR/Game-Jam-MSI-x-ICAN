using UnityEngine;
using System.Collections.Generic;

public class Recording
{
    public ReplayObject ReplayObject { get; private set; }

    private Queue<ReplayData> originalQueue;
    private Queue<ReplayData> replayQueue;

    public Recording(Queue<ReplayData> recordingQueue)
    {
        // On garde une copie permanente de l'enregistrement
        originalQueue = new Queue<ReplayData>(recordingQueue);

        // Cette queue sera consommée pendant le replay
        replayQueue = new Queue<ReplayData>(recordingQueue);
    }

    public void RestartFromBeginning()
    {
        // Recrée la queue à partir de l'enregistrement original
        replayQueue = new Queue<ReplayData>(originalQueue);
    }

    public bool PlayNextFrame()
    {
        if (ReplayObject == null)
        {
            Debug.LogError("ReplayObject is Null");
            return false;
        }

        if (replayQueue.Count == 0)
        {
            return false;
        }

        ReplayData data = replayQueue.Dequeue();

        ReplayObject.SetDataForFrame(data);

        return true;
    }

    public void InstantiateReplayObject(GameObject replayObjectPrefab)
    {
        if (replayQueue.Count == 0)
        {
            Debug.LogWarning("Cannot instantiate ReplayObject: recording is empty.");
            return;
        }

        // On regarde la première position enregistrée
        ReplayData startingData = replayQueue.Peek();

        // Création du clone à cette position
        GameObject replayObject = Object.Instantiate(
            replayObjectPrefab,
            startingData.position,
            Quaternion.identity
        );

        // Récupération du composant ReplayObject
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
        if (ReplayObject != null)
        {
            Object.Destroy(ReplayObject.gameObject);
            ReplayObject = null;
        }
    }
}