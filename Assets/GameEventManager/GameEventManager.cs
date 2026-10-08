using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }
    [SerializeField] private float loopDuration;
    public float time;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameEventManager");
        }

        instance = this;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > loopDuration)
        {
            CountdownEnds();
        }
    }


    public event Action onCountdownEnds;

    public void CountdownEnds()
    {
        time = 0;

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
