using UnityEngine;

public class PlayerReplayObject : ReplayObject
{
    public override void SetDataForFrame(ReplayData data)
    {
        PlayerReplayData playerData = (PlayerReplayData)data;
        transform.position = playerData.position;
        transform.rotation = playerData.dir;
    }
}
