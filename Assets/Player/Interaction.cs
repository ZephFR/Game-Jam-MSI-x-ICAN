
using UnityEngine;

public interface IInteractible
{
    public void OnInteract();
}

public class Interaction : MonoBehaviour
{

    [SerializeField] private Transform interactionOrigin;
    [SerializeField] private float interactionRange;
    [SerializeField] private KeyCode interactKey;
    
    [SerializeField] private Recorder recorder;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray r = new Ray(interactionOrigin.position, interactionOrigin.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactionRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractible interactible))
                {
                    interactible.OnInteract();
                    recorder.RecordInteraction(interactible);
                }
            }
        }
    }
}
