using UnityEngine;

public class LeverDoorController : MonoBehaviour
{
    [Header("Leviers")]
    public Lever lever1;
    public Lever lever2;

    [Header("Réglages du mouvement")]
    [Tooltip("Axe de glissement : Cochez si la porte doit glisser sur Z (forward) ou décochez pour X (right)")]
    public bool useForwardAxis = true;

    [Tooltip("Distance de glissement (Mettre une valeur négative si elle part dans le mauvais sens)")]
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
        if (lever1 == null || lever2 == null) return;
        
        bool shouldOpen = lever1.isActivated && lever2.isActivated;

        Vector3 targetPosition = shouldOpen ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }
}