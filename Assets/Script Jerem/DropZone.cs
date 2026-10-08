using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Tag de la boîte requise (ex: RedBox ou YellowBox)")]
    public string requiredBoxTag;

    [Tooltip("Point d'ancrage optionnel (laisser vide pour utiliser le centre du trigger)")]
    public Transform snapPoint;

    [HideInInspector]
    public bool isCorrectBoxInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredBoxTag))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                // 1. Immobilise la boîte
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                // 2. Aligne la boîte sur le snapPoint (ou le centre de la zone)
                Transform targetTransform = (snapPoint != null) ? snapPoint : transform;
                rb.transform.position = targetTransform.position;
                rb.transform.rotation = targetTransform.rotation;

                isCorrectBoxInside = true;
                Debug.Log($"Boîte {requiredBoxTag} verrouillée dans {gameObject.name} !");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si le joueur attrape à nouveau la boîte verrouillée
        if (other.CompareTag(requiredBoxTag))
        {
            isCorrectBoxInside = false;
        }
    }
}