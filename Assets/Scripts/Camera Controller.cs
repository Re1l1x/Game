using UnityEngine;

public class CameraController : MonoBehaviour {
    public float rotationSpeed = 2f;

    private Quaternion targetRotation;
    private bool isRotating;
    
    private InputSystem controls;

    private void Awake()
    {
        controls = new InputSystem();

        controls.Camera.RotateLeft.performed += ctx => OnRotateLeft();
        controls.Camera.RotateRight.performed += ctx => OnRotateRight();
    }

    private void OnEnable()
    {
        controls.Camera.Enable();
    }

    private void OnDisable()
    {
        controls.Camera.Disable();
    }

    private void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update() {
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    private void OnRotateLeft()
    {
        if (!isRotating)
        {
            targetRotation *= Quaternion.Euler(0, 90, 0);
            isRotating = true;
        }
    }

    private void OnRotateRight()
    {
        if (!isRotating)
        {
            targetRotation *= Quaternion.Euler(0, -90, 0);
            isRotating = true;
        }
    }
}
