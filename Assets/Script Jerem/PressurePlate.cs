using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isPressed = false;

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
        if (other.gameObject)
        {
            isPressed = true;
            plateRenderer.material.color = pressedColor;

            Debug.Log("Pressure Plate pressed");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject)
        {
            isPressed = false;
            plateRenderer.material.color = normalColor;

            Debug.Log("Pressure Plate released");
        }
    }
}