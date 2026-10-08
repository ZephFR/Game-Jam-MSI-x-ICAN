using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Réglages")]
    public Transform holdPoint;          
    public float grabRange = 5f;         
    public float throwForce = 15f;       
    public KeyCode grabKey = KeyCode.E;  

    private Rigidbody heldBox = null;
    private Collider heldCollider = null;

    private void Update()
    {
        // Appui sur E : Attraper ou Poser
        if (Input.GetKeyDown(grabKey))
        {
            if (heldBox == null)
            {
                TryGrabBox();
            }
            else
            {
                DropBox();
            }
        }

        // Clic Gauche : Lancer la boîte tenue
        if (heldBox != null && Input.GetMouseButtonDown(0))
        {
            ThrowBox();
        }

        // Maintient la boîte à la position d'accroche devant la caméra
        if (heldBox != null && holdPoint != null)
        {
            heldBox.MovePosition(holdPoint.position);
            heldBox.MoveRotation(holdPoint.rotation);
        }
    }

    private void TryGrabBox()
    {
        if (Camera.main == null) return;

        Vector3 rayOrigin = Camera.main.transform.position + (Camera.main.transform.forward * 0.5f);
        Ray ray = new Ray(rayOrigin, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            if (hit.collider.CompareTag("RedBox") || hit.collider.CompareTag("YellowBox"))
            {
                Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();
                if (rb != null)
                {
                    heldBox = rb;
                    heldCollider = hit.collider;

                    // 1. Désactive la physique pendant le transport
                    heldBox.isKinematic = true;

                    // 2. Désactive la collision pour que la boîte ne bloque PAS le joueur quand il avance
                    if (heldCollider != null)
                    {
                        heldCollider.enabled = false;
                    }
                }
            }
        }
    }

    private void DropBox()
    {
        if (heldBox == null) return;

        // Réactive la physique et les collisions
        heldBox.isKinematic = false;
        
        if (heldCollider != null)
        {
            heldCollider.enabled = true;
            heldCollider = null;
        }

        heldBox = null;
    }

    private void ThrowBox()
    {
        if (heldBox == null) return;

        Rigidbody rbToThrow = heldBox;
        Collider colToThrow = heldCollider;

        // On libère la boîte
        heldBox = null;
        heldCollider = null;

        // On réactive la physique et la collision
        rbToThrow.isKinematic = false;
        if (colToThrow != null)
        {
            colToThrow.enabled = true;
        }

        // On applique l'impulsion vers l'avant de la caméra
        rbToThrow.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
    }
}