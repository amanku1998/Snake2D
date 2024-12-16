using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpController : MonoBehaviour
{
    public string powerUpType; // e.g., "Shield", "ScoreBoost", "SpeedUp"
    public bool HasBeenActivated { get; set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (collision.CompareTag("Player")) // Assuming the snake head is tagged as "Player"
            {
                SnakeController snake = collision.GetComponent<SnakeController>();
                if (snake != null)
                {
                    Destroy(gameObject); // Remove the power-up after collection
                }
            }
        }
        else
        {
            if (collision.CompareTag("Snake1") || collision.CompareTag("Snake2"))
            {
                SnakeController1 snake = collision.GetComponent<SnakeController1>();
                if (snake != null)
                {
                    Destroy(gameObject); // Remove the power-up after collection
                }
            }
        }
    }
}
