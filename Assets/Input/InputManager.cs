using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private bool jumpPressed = false;
    private bool submitPressed = false;

    public static InputManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance");
        }

        instance = this;
    }


    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector3>();
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector3>();
        }
    }
    
    public void JumpPressed (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }
    
    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
    
    public bool GetJumpPressed() 
    {
        bool result = jumpPressed;
        RegisterJumpPressedThisFrame();
        return result;
    }

    public bool GetSubmitPressed() 
    {
        bool result = submitPressed;
        RegisterSubmitPressedThisFrame();
        return result;
    }

    public void RegisterSubmitPressedThisFrame() 
    {
        submitPressed = false;
    }

    public void RegisterJumpPressedThisFrame() 
    {
        jumpPressed = false;
    }

}