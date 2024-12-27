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
        if (collision.CompareTag("Snake1") || collision.CompareTag("Snake2"))
        {
            SnakeController snake = collision.GetComponent<SnakeController>();
            if (snake != null)
            {
                Destroy(gameObject); // Remove the power-up after collection
            }
        }
    }
}
