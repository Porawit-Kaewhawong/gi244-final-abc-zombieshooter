using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;

    private InputAction moveAction;
    
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        var moveInput = moveAction.ReadValue<Vector2>();
        var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        transform.Translate(moveDirection * walkSpeed * Time.deltaTime);
    }
}
