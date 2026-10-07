using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public bool isActivated = false;

    [Header("Visuel (Optionnel)")]
    public Renderer zoneRenderer;
    public Color activeColor = Color.green;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet qui entre a le script CannonBall
        if (other.GetComponent<CannonBall>() != null || other.name.Contains("Ball"))
        {
            isActivated = true;
            Debug.Log("Cible atteinte par la balle !");

            if (zoneRenderer != null)
            {
                zoneRenderer.material.color = activeColor;
            }

            // Optionnel : Détruire la balle une fois la zone atteinte
            Destroy(other.gameObject);
        }
    }
}