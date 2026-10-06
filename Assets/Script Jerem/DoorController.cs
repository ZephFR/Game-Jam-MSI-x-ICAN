using UnityEngine;

public class DoorController : MonoBehaviour
{
    public PressurePlate plate1;
    public PressurePlate plate2;

    public Transform openPosition;
    public Transform closedPosition;

    public float speed = 2f;

    void Update()
    {
        if (plate1.isPressed && plate2.isPressed)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                openPosition.position,
                speed * Time.deltaTime
            );
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                closedPosition.position,
                speed * Time.deltaTime
            );
        }
    }
}