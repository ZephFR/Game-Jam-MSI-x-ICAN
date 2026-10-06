using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameEventManager");
        }

        instance = this;
    }

    public event Action onCountdownEnds;

    public void CountdownEnds()
    {
        if (onCountdownEnds != null)
            onCountdownEnds();
    }
    
    public event Action onRestartLevel;

    public void RestartLevel()
    {
        if (onRestartLevel != null)
            onRestartLevel();
    }
    
    public event Action onPlayerRespawn;

    public void PlayerRespawn()
    {
        if (onPlayerRespawn != null)
            onPlayerRespawn();
    }
}
