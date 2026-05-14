using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int health = 5;
    public float walkSpeed = 5.0f;

    public Camera mainCamera;
    public TextMeshProUGUI healthText;

    [Header("Normal Bullet")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;

    [Header("Charge Bullet")]
    public GameObject chargeBulletPrefab;
    public float chargeTime = 3f;
    public float chargeBulletSpeed = 20f;

    private float currentCharge;
    private bool isCharging = false;

    [Header("Bomber Bullet")]
    public GameObject bomberBulletPrefeb;
    public float bomberBulletSpeed = 15f;
    public float bomberCooldown = 10f;

    private float nextbomberTime;

    [Header("Homing Bullet")]
    public GameObject homingBulletPrefab;
    public float homingBulletSpeed = 15f;
    public float homingCooldown = 5f;

    private float nextHomingTime;

    private float immunityFrame = 0.5f;
    private float nextFireTime;
    private float nextDamageTime;
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

        healthText.text = "Health " + health;
    }

    void Update()
    {
        // Movement
        var moveInput = moveAction.ReadValue<Vector2>();
        var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (rb.linearVelocity.magnitude < walkSpeed)
        {
            rb.AddForce(moveDirection * walkSpeed);
        }

        // Fire Action
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            FireProjectile(bulletPrefab, bulletSpeed);

            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            isCharging = true;
            currentCharge = 0f;
        }

        if (isCharging)
        {
            currentCharge += Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire2") && isCharging)
        {
            isCharging = false;

            if (currentCharge >= chargeTime)
            {
                FireProjectile(chargeBulletPrefab, chargeBulletSpeed);
            }
        }

        if (Input.GetMouseButtonDown(2) && Time.time >= nextbomberTime)
        {
            FireProjectile(bomberBulletPrefeb, bomberBulletSpeed);

            nextbomberTime = Time.time + bomberCooldown;
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextHomingTime)
        {
            FireProjectile(homingBulletPrefab, homingBulletSpeed);

            nextHomingTime = Time.time + homingCooldown;
        }
    }

    private void FireProjectile(GameObject selectedBullet, float selectedSpeed)
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        Vector3 dir = (mousePos - screenPos).normalized;

        GameObject bullet = Instantiate(selectedBullet, transform.position, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(dir.x, 0, dir.y) * selectedSpeed;
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

        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
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

    private void TakeDamage(int damage)
    {
        if (Time.time >= nextDamageTime)
        {
            if (health > 0)
            {
                health -= damage;
                healthText.text = "Health " + health;

                nextDamageTime = Time.time + immunityFrame;
            }
            else
            {
                GameManager.instance.RestartGame();
            }
        }
    }
}