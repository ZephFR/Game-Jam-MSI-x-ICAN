using UnityEngine;

public class SetCheckpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            // Move the respawn point to the checkpoint
            respawnPoint.position = transform.position;

            // Restart the level
            GameEventManager.instance.RestartLevel();
        }
    }
}