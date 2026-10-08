using UnityEngine;

public class PlayerReplayData : ReplayData
{
    public Quaternion dir { get;private set; }
    public IInteractible interactedObject  { get;private set; }

    public PlayerReplayData(Vector3 position, Quaternion dir, IInteractible interactedObject  = null)
    {
        this.position = position;
        this.dir = dir;
        this.interactedObject = interactedObject;
    }
}
