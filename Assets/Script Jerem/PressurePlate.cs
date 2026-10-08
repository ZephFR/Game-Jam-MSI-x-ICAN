using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isPressed = false;
    private int objectsOnPlate = 0;

    private Renderer plateRenderer;
    public Color normalColor = Color.red;
    public Color pressedColor = Color.green;

    private void Start()
    {
        plateRenderer = GetComponent<Renderer>();
        plateRenderer.material.color = normalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        objectsOnPlate++;
        UpdateState();
    }

    private void OnTriggerExit(Collider other)
    {
        objectsOnPlate--;
        if (objectsOnPlate < 0) objectsOnPlate = 0;
        UpdateState();
    }

    private void UpdateState()
    {
        isPressed = objectsOnPlate > 0;
        plateRenderer.material.color = isPressed ? pressedColor : normalColor;
    }
}