using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Déclencheurs")]
    public PressurePlate plate1;
    public PressurePlate plate2;

    [Header("Réglages du mouvement")]
    [Tooltip("Distance de glissement le long du mur")]
    public float openDistance = 3f;

    [Tooltip("Vitesse de déplacement")]
    public float speed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = transform.position;

        
        openPosition = closedPosition + (transform.forward * openDistance);
    }

    private void Update()
    {
        if (plate1 == null || plate2 == null) return;

        bool bothPressed = plate1.isPressed && plate2.isPressed;
        Vector3 targetPosition = bothPressed ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }
}