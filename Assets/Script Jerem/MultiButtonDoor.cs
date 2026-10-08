using UnityEngine;

public class MultiButtonDoor : MonoBehaviour
{
    [Header("Boutons requis")]
    public PressureButton button1;
    public PressureButton button2;
    public PressureButton button3;

    [Header("Réglages du mouvement")]
    public float openDistance = 3f;
    public float speed = 5f;

    [Tooltip("Cochez si la porte doit rester ouverte définitivement une fois déclenchée")]
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
        if (button1 == null || button2 == null || button3 == null) return;

        // La porte vérifie si tous les boutons sont passés à l'état activé
        bool allPressed = button1.isPressed && button2.isPressed && button3.isPressed;

        if (allPressed)
        {
            hasBeenOpened = true;
        }

        bool shouldOpen = stayOpenPermanent ? hasBeenOpened : allPressed;
        Vector3 targetPosition = shouldOpen ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }
}