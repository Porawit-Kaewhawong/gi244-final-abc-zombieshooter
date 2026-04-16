using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;

    private float defaultSpeed;
    private Coroutine speedCoroutine;
    private InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        defaultSpeed = walkSpeed;
    }

    void Update()
    {
        var moveInput = moveAction.ReadValue<Vector2>();
        var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        transform.Translate(moveDirection * walkSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if (speedCoroutine != null)
            {
                StopCoroutine(speedCoroutine);
            }
            StartCoroutine(SpeedBuff());

            Destroy(other.gameObject);
        }
    }

    private IEnumerator SpeedBuff()
    {
        walkSpeed = defaultSpeed * 2f;

        yield return new WaitForSeconds(5f);

        walkSpeed = defaultSpeed;
    }
}