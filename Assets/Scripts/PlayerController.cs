using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float SprintSpeed = 20;

    private CharacterController movementController;
    private Camera cam;

    private float currMoveSpeed;
    private Vector3 currFallingSpeed;
    private Vector3 moveVelocity;
    private Vector3 moveDampVelocity;
    public float moveSmoothTime = 0.1f;
    
    private InputSystem inputSystem;
    private InputAction moveAction;
    private InputAction sprintAction;

    private void OnEnable() {
        moveAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable() {
        moveAction.Disable();
        sprintAction.Disable();
    }

    private void Awake()
    {
        inputSystem = new InputSystem();
        moveAction = new InputAction();
        sprintAction = new InputAction();
        moveAction = inputSystem.FindAction("Move");
        sprintAction = inputSystem.FindAction("Sprint");
    }

    protected virtual void Start()
    {
        movementController = GetComponent<CharacterController>(); //  Character Controller
        cam = Camera.main;
    }

    private void Update() {
        // if (Input.GetKeyDown(KeyCode.R))
        //     transform.position = new Vector3(3, 0, 0); // Hit "R" to spawn in this position
        Vector3 moveDirection = Vector3.zero;
        if (moveAction.IsInProgress())
        {
            moveDirection += (cam.transform.forward-new Vector3(0,cam.transform.forward.y,0)) * moveAction.ReadValue<Vector2>().y;
            moveDirection += (cam.transform.right-new Vector3(0,cam.transform.right.y,0)) * moveAction.ReadValue<Vector2>().x;
            moveDirection.Normalize();
        }

        if (movementController.isGrounded)
        {
            currFallingSpeed = Vector3.zero;
        }
        else
        {
            currFallingSpeed += -transform.up * (9.81f * 10) * Time.deltaTime; // Gravity
        }

        if (sprintAction.IsInProgress())
        {  // Player can sprint by holding "Left Shit" keyboard button
            currMoveSpeed = SprintSpeed;
        }
        else
        {
            currMoveSpeed = MoveSpeed;
        }
        
        moveVelocity = Vector3.SmoothDamp(moveVelocity, moveDirection*currMoveSpeed, ref moveDampVelocity, moveSmoothTime );
        
        moveVelocity += currFallingSpeed;
        
        
        
        movementController.Move(moveVelocity* Time.deltaTime);
    }
}
