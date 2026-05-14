using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingBullet : MonoBehaviour
{
    [Header("Homing Bullet Settings")]
    public float rotateSpeed = 300f;
    public float lifeTime = 5f;

    private PlayerController player;
    private Transform target;
    private Rigidbody rb;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, lifeTime);

        if (rb.linearVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    void Update()
    {
        FindClosestEnemy();

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotateSpeed * Time.deltaTime
                );
            }
        }

        rb.linearVelocity = transform.forward * player.homingBulletSpeed;
    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }
}