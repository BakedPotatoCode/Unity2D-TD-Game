// Finish.cs
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public Text healthText;
    public int health = 100;

    void Start()
    {
        // Update the health text initially
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        health--;
        Singleton.Instance.EnemiesRemaining--;

        Destroy(other.gameObject);
        UpdateHealthText();

    }

}