using UnityEngine;

public class ButtonDoorController : MonoBehaviour
{
    [Header("Boutons requis")]
    public ButtonInteractable button1;
    public ButtonInteractable button2;

    [Header("Réglages du mouvement")]
    [Tooltip("Distance de glissement le long du mur (Axe Z)")]
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
        if (button1 == null || button2 == null) return;

        
        bool shouldOpen = button1.isPressed && button2.isPressed;

        Vector3 targetPosition = shouldOpen ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }
}