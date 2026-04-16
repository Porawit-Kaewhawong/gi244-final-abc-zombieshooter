using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> AllEnemies = new List<Enemy>();
    public static bool isSlowDown = false;

    public float walkSpeed = 3.0f;

    private Rigidbody rb;
    private GameObject player;

    private void OnEnable()
    {
        AllEnemies.Add(this);
    }

    private void OnDisable()
    {
        AllEnemies.Remove(this);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
    }

    
    void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        float currentSpeed = walkSpeed;

        if (isSlowDown)
        {
            currentSpeed *= 0.5f;
        }

        if (rb.linearVelocity.magnitude < currentSpeed)
        {
            rb.AddForce(direction * currentSpeed);
        }
    }
}
