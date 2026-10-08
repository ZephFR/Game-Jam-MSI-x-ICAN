using UnityEngine;

public class ObjectReplayData : ReplayData
{
    public bool isActive {  get; private set; }

    public ObjectReplayData(bool isActive)
    {
        this.isActive = isActive;
    }
}
