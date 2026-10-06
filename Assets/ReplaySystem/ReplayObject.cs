using UnityEngine;

public class ReplayObject : MonoBehaviour
{
    public void SetDataForFrame(ReplayData data)
    {
        transform.position = data.position;
    }
}