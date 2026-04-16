using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetPosition;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }

    void LateUpdate()
    {
        transform.position = targetPosition.position + offset;
    }
}
