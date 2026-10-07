using UnityEngine;

public class ButtonInteractable : MonoBehaviour
{
    public bool isPressed = false;

    [Header("Visuel (Optionnel)")]
    public Transform buttonMesh; 
    public float pressDepth = 0.1f; 

    private Vector3 unpressedLocalPos;
    private Vector3 pressedLocalPos;
    private bool isPlayerNearby = false;

    private void Start()
    {
        if (buttonMesh != null)
        {
            unpressedLocalPos = buttonMesh.localPosition;
            
            pressedLocalPos = unpressedLocalPos - new Vector3(0f, pressDepth, 0f);
        }
    }

    private void Update()
    {
        
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleState();
        }
    }

    private void ToggleState()
    {
        isPressed = !isPressed;

        if (buttonMesh != null)
        {
            buttonMesh.localPosition = isPressed ? pressedLocalPos : unpressedLocalPos;
        }

        Debug.Log($"{gameObject.name} appuyé : {isPressed}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null)
        {
            isPlayerNearby = false;
        }
    }

    private void OnGUI()
    {
        if (isPlayerNearby)
        {
            GUI.Label(new Rect(Screen.width / 2f - 100, Screen.height / 2f + 50, 200, 30), "Appuie sur [E] pour appuyer sur le bouton");
        }
    }
}