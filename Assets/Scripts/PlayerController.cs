using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float SprintSpeed = 30;
    private float currMoveSpeed = 0;

    private CharacterController movementController;
    private Camera cam;

    protected Vector3 velocity;
    
    private InputSystem inputSystem;
    private InputAction moveAction;
    private InputAction sprintAction;

    private void OnEnable() {
        moveAction.Enable();
    }

    private void OnDisable() {
        moveAction.Disable();
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
        Vector3 direction = Vector3.zero;
        if (moveAction.IsInProgress())
        {
            direction += (cam.transform.forward-new Vector3(0,cam.transform.forward.y,0)) * moveAction.ReadValue<Vector2>().y;
            direction += (cam.transform.right-new Vector3(0,cam.transform.right.y,0)) * moveAction.ReadValue<Vector2>().x;
            direction.Normalize();
        }

        if (movementController.isGrounded)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity += -transform.up * (9.81f * 10) * Time.deltaTime; // Gravity
        }

        if (sprintAction.IsPressed())
        {  // Player can sprint by holding "Left Shit" keyboard button
            currMoveSpeed = SprintSpeed;
        }
        else
        {
            currMoveSpeed = MoveSpeed;
        }

        direction += velocity * Time.deltaTime;
        movementController.Move(direction * Time.deltaTime * currMoveSpeed);
    }
}
