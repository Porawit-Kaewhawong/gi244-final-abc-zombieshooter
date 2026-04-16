using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int hp = 5;
    public TextMeshProUGUI healthText;
    public float walkSpeed = 5.0f;

    private float defaultSpeed;
    private Rigidbody rb;
    private Coroutine speedCoroutine;
    private Coroutine slowCoroutine;
    private InputAction moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        
        defaultSpeed = walkSpeed;

        healthText.text = "Health " + hp;
    }

    void Update()
    {
        var moveInput = moveAction.ReadValue<Vector2>();
        var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (rb.linearVelocity.magnitude < walkSpeed)
        {
            rb.AddForce(moveDirection * walkSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if (speedCoroutine != null)
            {
                StopCoroutine(speedCoroutine);
            }
            speedCoroutine = StartCoroutine(SpeedBuff());

            Destroy(other.gameObject);
        }

        if (other.CompareTag("EnemyDebuff"))
        {
            if (slowCoroutine != null)
            {
                StopCoroutine(slowCoroutine);
            }
            slowCoroutine = StartCoroutine(EnemySlowDown());

            Destroy(other.gameObject);
        }

    }

    private IEnumerator SpeedBuff()
    {
        walkSpeed = defaultSpeed * 1.5f;

        yield return new WaitForSeconds(5f);

        walkSpeed = defaultSpeed;
    }

    private IEnumerator EnemySlowDown()
    {
        Enemy.isSlowDown = true;

        yield return new WaitForSeconds(7.5f);

        Enemy.isSlowDown = false;
    }
}