using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public bool isPressed = false;
    public float interactDistance = 3f;

    [Header("Visuel")]
    public Color pressedColor = Color.green;

    private Camera mainCamera;
    private Renderer buttonRenderer;

    private void Start()
    {
        mainCamera = Camera.main;
        buttonRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Si déjà pressé, inutile de refaire la détection
        if (isPressed) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (mainCamera == null) return;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // Vérifie si le Raycast a touché CET objet (ou un enfant)
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                ActivateButton();
            }
        }
    }

    private void ActivateButton()
    {
        isPressed = true;

        // Change la couleur du matériau en vert
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = pressedColor;
        }

        Debug.Log("Bouton activé et passé en vert !");
    }
}