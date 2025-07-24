using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10;
    [SerializeField] private float SprintSpeed = 20;

    private CharacterController movementController;
    private Camera cam;
    private Vector3 moveDirection;

    private float currMoveSpeed;
    private Vector3 currFallingSpeed;
    private Vector3 moveVelocity;
    private Vector3 moveDampVelocity;
    [SerializeField] private float moveSmoothTime = 0.1f;
    
    [SerializeField] private float roatationSpeed = 1f;
    
    private InputSystem inputSystem;
    private InputAction moveAction;
    private InputAction sprintAction;

    private void Awake()
    {
        inputSystem = new InputSystem();
        moveAction = new InputAction();
        sprintAction = new InputAction();
        
        moveAction = inputSystem.FindAction("Move");
        sprintAction = inputSystem.FindAction("Sprint");
    }
    
    private void OnEnable() { 
        moveAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable() { //а что если action используется в двух местах и где то он отключается а где то нет?
        moveAction.Disable();
        sprintAction.Disable();
    }

    protected virtual void Start()
    {
        movementController = GetComponent<CharacterController>();
        cam = Camera.main;
        moveDirection = Vector3.zero;
    }

    private void Update() {
        // if (Input.GetKeyDown(KeyCode.R))
        //     transform.position = new Vector3(3, 0, 0); // Hit "R" to spawn in this position
        moveDirection *= 0;
        if (moveAction.IsInProgress())
        {
            Move();
            Rotate();
        }
        
        moveVelocity = Vector3.SmoothDamp(moveVelocity, moveDirection*currMoveSpeed, ref moveDampVelocity, moveSmoothTime );

        Fall();
        
        moveVelocity += currFallingSpeed;
        
        movementController.Move(moveVelocity* Time.deltaTime);
    }

    public void Move()
    {
        moveDirection += (cam.transform.forward-new Vector3(0,cam.transform.forward.y,0)) * moveAction.ReadValue<Vector2>().y;
        moveDirection += (cam.transform.right-new Vector3(0,cam.transform.right.y,0)) * moveAction.ReadValue<Vector2>().x;
        moveDirection.Normalize();
            
        if (sprintAction.IsInProgress())
        {
            currMoveSpeed = SprintSpeed;
        }
        else
        {
            currMoveSpeed = MoveSpeed;
        }
    }

    public void Rotate()
    {
        float maxRotationStep=roatationSpeed*Time.deltaTime;
        transform.rotation=Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), roatationSpeed);
    }

    public void Fall() //почему то слишком быстро падает
    {
        if (movementController.isGrounded)
        {
            currFallingSpeed = Vector3.zero;
        }
        else
        {
            currFallingSpeed += -transform.up * (9.81f * 10) * Time.deltaTime; // Gravity
        }
    }
}
