using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("Paramètres")]
    public float lifetime = 8f; 

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}