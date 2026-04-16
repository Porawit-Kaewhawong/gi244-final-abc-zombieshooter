using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetPosition;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - targetPosition.position;
    }

    void LateUpdate()
    {
        if (targetPosition != null)
        {
            transform.position = targetPosition.position + offset;
        }
    }
}
