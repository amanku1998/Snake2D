using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpController : MonoBehaviour
{
    public ItemType powerUpType; // e.g., "Shield", "ScoreBoost", "SpeedUp"
    public bool HasBeenActivated { get; set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeController snake = collision.GetComponent<SnakeController>();
        if (snake != null)
        {
            Destroy(gameObject); // Remove the power-up after collection
        }
    }
}
