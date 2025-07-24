using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraScroll : MonoBehaviour
{
    private InputSystem inputSystem;
    private InputAction zoomAction;
    
    private float zoomAmount=0.5f;
    [SerializeField] private float zoomSmoothTime=0.1f;
    [SerializeField] private float zoomAvailablePos=10;
    
    [SerializeField] private Vector3 closestPosition;
    [SerializeField] private Vector3 farestPosition;
    
    [SerializeField] private Transform cameraPosition;
    
    private Vector3 currPosition;
    private Vector3 dampVelocity;
    
    private void Awake()    
    {
        inputSystem = new InputSystem();
        zoomAction = new InputAction();
        zoomAction = inputSystem.FindAction("Zoom");
    }
    
    private void OnEnable() {
        zoomAction.Enable();
    }

    private void OnDisable() { //а что если action используется в двух местах и где то он отключается а где то нет?
        zoomAction.Disable();
    }

    private void Start()
    {
        zoomAmount = 0.5f;
        
    }

    void Update() //наверное ещё надо менять скорость слежения камеры (другой скрипт) потому что вблизи это очень заметно
    {
        if (zoomAction.IsInProgress())
        {
            zoomAmount += zoomAction.ReadValue<Vector2>().y/zoomAvailablePos;
            zoomAmount = Mathf.Clamp(zoomAmount, 0, 1);
        }
        Vector3 nextPosition = Vector3.Lerp( farestPosition, closestPosition, zoomAmount );
        currPosition = Vector3.SmoothDamp(cameraPosition.localPosition, nextPosition, ref dampVelocity, zoomSmoothTime );
        
        cameraPosition.localPosition = currPosition;
    }
}
