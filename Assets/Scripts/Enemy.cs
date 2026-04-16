using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkSpeed = 3.0f;

    private Rigidbody rb;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
    }

    
    void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        if (rb.linearVelocity.magnitude < walkSpeed)
        {
            rb.AddForce(direction * walkSpeed);
        }
    }
}
