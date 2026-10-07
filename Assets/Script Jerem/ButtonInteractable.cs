using System.Collections;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour, IInteractible
{
    public bool isPressed = false;

    [Header("Réglages")]
    public float resetDelay = 0.5f; 

    [Header("Visuel (Optionnel)")]
    public Renderer buttonRenderer;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;

    private Coroutine resetCoroutine;

    private void Start()
    {
        UpdateVisual();
    }

    public void OnInteract()
    {
        
        if (isPressed) return;

        isPressed = true;
        UpdateVisual();

        
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        
        resetCoroutine = StartCoroutine(ResetButtonAfterDelay());
    }

    private IEnumerator ResetButtonAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        isPressed = false;
        UpdateVisual();
        Debug.Log($"{gameObject.name} s'est désactivé (délai écoulé).");
    }

    private void UpdateVisual()
    {
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = isPressed ? activeColor : inactiveColor;
        }
    }
}