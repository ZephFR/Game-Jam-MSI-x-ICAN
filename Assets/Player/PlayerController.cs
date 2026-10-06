using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
     [SerializeField] private float walkSpeed = 6f;
     
     [SerializeField] private float runSpeed = 9f;
     
     [SerializeField] private float jumpForce = 8f;
     [SerializeField] private float gravity = 20f;
     
     [SerializeField] private float lookSensitivity = 0.2f;
     [SerializeField] private float lookAngleLimit = 90f;
     
     private Camera cam;
     private CharacterController controller;

     private InputAction moveInput;
     private InputAction runInput;
     private InputAction jumpInput;
     private bool jumped = false;
     private float currentMoveSpeed;
     private Vector3 moveDirection = Vector3.zero;
     private float lookAngle;

     private void Awake()
     {
          cam = GetComponentInChildren<Camera>();
          controller = GetComponent<CharacterController>();
          moveInput = InputSystem.actions.FindAction("Move");
          runInput = InputSystem.actions.FindAction("Sprint");
          jumpInput = InputSystem.actions.FindAction("Jump");
          jumpInput.started += Jumped;

          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;

          currentMoveSpeed = walkSpeed;
     }

     private void Update()
     {
          Vector2 moveVector = moveInput.ReadValue<Vector2>();
          Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());
          
          currentMoveSpeed = runInput.IsPressed() ? runSpeed : walkSpeed;
          HandleMovement(moveVector);
          HandleLooking(mouseDelta);

          if (!controller.isGrounded)
               jumped = false;
     }

     private void HandleMovement(Vector2 moveVector)
     {
          Vector3 forward = transform.TransformDirection(Vector3.forward);
          Vector3 right = transform.TransformDirection(Vector3.right);

          float oldY = moveDirection.y;

          Vector2 newSpeed = new Vector2(moveVector.y * currentMoveSpeed, moveVector.x * currentMoveSpeed);

          moveDirection = (forward * newSpeed.x) + (right * newSpeed.y);
          moveDirection.y = (jumped && controller.isGrounded) ? jumpForce : oldY;

          controller.Move(moveDirection * Time.deltaTime);

          if (!controller.isGrounded)
               moveDirection.y -= gravity * Time.deltaTime;
     }

     private void HandleLooking(Vector2 mouseDelta)
     {
          lookAngle += -mouseDelta.y * lookSensitivity;
          lookAngle = Mathf.Clamp(lookAngle, -lookAngleLimit, lookAngleLimit);

          cam.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
          transform.rotation *= Quaternion.Euler(0, mouseDelta.x * lookSensitivity, 0);
     }

     private void Jumped(InputAction.CallbackContext _)
     {
          jumped = true;
     }
}