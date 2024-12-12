using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public string powerUpType; // e.g., "Shield", "ScoreBoost", "SpeedUp"

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Assuming the snake head is tagged as "Player"
        {
            SnakeController snake = collision.GetComponent<SnakeController>();
            if (snake != null)
            {
                snake.ActivatePowerUp(powerUpType);
                // Notify Food to clear the reference to this power-up
                Food food = FindObjectOfType<Food>();
                if (food != null)
                {
                    food.ClearCurrentPowerUp();
                }

                Destroy(gameObject); // Remove the power-up after collection
            }
        }
    }
}
