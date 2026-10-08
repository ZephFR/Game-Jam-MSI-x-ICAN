using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerReplayObject : ReplayObject
{
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void SetDataForFrame(ReplayData data)
    {
        PlayerReplayData playerData = (PlayerReplayData)data;
        transform.position = playerData.position;   
        transform.rotation = playerData.dir;

        if (playerData.interactedObject != null)
        {
            playerData.interactedObject.OnInteract();
        }
        
        anim.SetFloat("Velocity", playerData.velocity);

        if (playerData.jumped)
        {
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }
    }
}
