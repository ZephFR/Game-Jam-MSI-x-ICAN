using UnityEngine;

public class PlayerReplayData : ReplayData
{
    public Quaternion dir { get;private set; }

    public PlayerReplayData(Vector3 position, Quaternion dir)
    {
        this.position = position;
        this.dir = dir;
    }
}
