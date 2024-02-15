using UnityEngine;
using System;
public class Enemy : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    // Define the event for enemy death
    public event Action<GameObject> OnEnemyDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Notify subscribers about enemy death
            OnEnemyDeath?.Invoke(gameObject);
            Singleton.Instance.CoinsInPocket += 1;

            // Handle other logic if needed
            Destroy(gameObject);
        }
    }
}
