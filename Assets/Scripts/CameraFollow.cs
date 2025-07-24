using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float smoothingSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 nextPosition = Vector3.Lerp(
            transform.position,
            targetTransform.position,
            Time.deltaTime*smoothingSpeed*(1+(transform.position-targetTransform.position).magnitude));
        
        transform.position = nextPosition;
    }
}
