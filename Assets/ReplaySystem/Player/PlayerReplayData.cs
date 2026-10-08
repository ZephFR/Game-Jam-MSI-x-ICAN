using UnityEngine;

public class PlayerReplayData : ReplayData
{
    public Quaternion dir { get;private set; }
    public IInteractible interactedObject  { get;private set; }
    public bool jumped { get; private set; }
    public float velocity { get; private set; }

    public PlayerReplayData(Vector3 position, Quaternion dir, bool jumped, float velocity, IInteractible interactedObject = null)
    {
        this.position = position;
        this.dir = dir;
        this.interactedObject = interactedObject;
        this.jumped = jumped;
        this.velocity = velocity;
    }
}
