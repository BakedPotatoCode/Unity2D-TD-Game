using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 50;
    public float lifetime = 2f; // Lifetime of the projectile in seconds

    private float timer;

    void Start()
    {
        timer = lifetime;
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        // Decrease the timer
        timer -= Time.deltaTime;
        // Destroy the projectile if the timer reaches zero
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
