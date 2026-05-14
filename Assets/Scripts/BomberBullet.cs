using UnityEngine;

public class BomberBullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float explosionRadius = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in enemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                // 1. Call the GameManager to increase the score
                if (GameManager.instance != null)
                {
                    GameManager.instance.AddKill();
                }

                Destroy(hit.gameObject);
            }
        }
    }
}