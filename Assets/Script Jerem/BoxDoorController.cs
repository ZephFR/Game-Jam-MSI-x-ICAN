using UnityEngine;

public class BoxDoorController : MonoBehaviour
{
    [Header("Zones requises")]
    public DropZone redZone;
    public DropZone yellowZone;

    [Header("Réglages du mouvement")]
    [Tooltip("Distance de glissement le long du mur (Axe Z / forward)")]
    public float openDistance = 3f;

    [Tooltip("Vitesse de déplacement de la porte")]
    public float speed = 3f;

    [Tooltip("Cochez pour que la porte reste ouverte une fois déverrouillée")]
    public bool stayOpenPermanent = true;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool hasBeenOpened = false;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + (transform.forward * openDistance);
    }

    private void Update()
    {
        if (redZone == null || yellowZone == null) return;

        // Vérifie si les deux boîtes sont dans leurs zones respectives en même temps
        if (redZone.isCorrectBoxInside && yellowZone.isCorrectBoxInside)
        {
            hasBeenOpened = true;
        }

        bool shouldOpen = stayOpenPermanent ? hasBeenOpened : (redZone.isCorrectBoxInside && yellowZone.isCorrectBoxInside);
        Vector3 targetPosition = shouldOpen ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }
}