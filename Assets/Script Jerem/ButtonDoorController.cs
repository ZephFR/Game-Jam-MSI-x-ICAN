using UnityEngine;

public class ButtonDoorController : MonoBehaviour
{
    [Header("Boutons requis")]
    public ButtonInteractable button1;
    public ButtonInteractable button2;

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
        if (button1 == null || button2 == null) return;

        
        if (button1.isPressed && button2.isPressed)
        {
            hasBeenOpened = true;
        }

        bool shouldOpen = stayOpenPermanent ? hasBeenOpened : (button1.isPressed && button2.isPressed);
        Vector3 targetPosition = shouldOpen ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }
}