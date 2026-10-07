using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isActivated = false;

    [Header("Visuel (Optionnel)")]
    public Transform handle; // La poignée du levier
    public float activeAngle = -45f;
    public float inactiveAngle = 45f;

    private bool isPlayerNearby = false;

    private void Update()
    {
        
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLever();
        }
    }

    private void ToggleLever()
    {
        isActivated = !isActivated;
        Debug.Log($"{gameObject.name} activé : {isActivated}");

        
        if (handle != null)
        {
            float targetAngle = isActivated ? activeAngle : inactiveAngle;
            handle.localRotation = Quaternion.Euler(targetAngle, 0f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.GetComponent<CharacterController>() != null || other.GetComponent<Rigidbody>() != null)
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null || other.GetComponent<Rigidbody>() != null)
        {
            isPlayerNearby = false;
        }
    }

    private void OnGUI()
    {
        
        if (isPlayerNearby)
        {
            GUI.Label(new Rect(Screen.width / 2f - 100, Screen.height / 2f + 50, 200, 30), "Appuie sur [E] pour interagir");
        }
    }
}