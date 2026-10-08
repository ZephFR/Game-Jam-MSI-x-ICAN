using UnityEngine;

public class TargetDoorController : MonoBehaviour
{
    [Header("Déclencheur")]
    public TargetZone targetZone;

    [Header("Réglages du mouvement")]
    [Tooltip("Axe de glissement : Cochez si la porte doit glisser sur Z (forward) ou décochez pour X (right)")]
    public bool useForwardAxis = true;

    [Tooltip("Distance de glissement")]
    public float openDistance = 3f;

    [Tooltip("Vitesse de déplacement")]
    public float speed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = transform.position;

        Vector3 direction = useForwardAxis ? transform.forward : transform.right;
        openPosition = closedPosition + (direction * openDistance);
    }

    private void Update()
    {
        if (targetZone == null) return;

        bool shouldOpen = targetZone.isActivated;
        Vector3 targetPos = shouldOpen ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );
    }
}