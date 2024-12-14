using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public string powerUpType; // e.g., "Shield", "ScoreBoost", "SpeedUp"
    public bool HasBeenActivated { get; set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Assuming the snake head is tagged as "Player"
        {
            SnakeController snake = collision.GetComponent<SnakeController>();
            if (snake != null)
            {
                //snake.ActivatePowerUp(powerUpType);
                //FoodManager food = FindObjectOfType<FoodManager>();
                //if (food != null)
                //{
                //    food.ClearCurrentPowerUp();
                //}

                // Activate the power-up in the FoodManager
                FoodManager foodManager = FindObjectOfType<FoodManager>();
                //foodManager.ActivatePowerUp(powerUpType);

                Destroy(gameObject); // Remove the power-up after collection
            }
        }
    }

    //public void ResetPowerUpValue()
    //{
    //    HasBeenActivated = false;
    //}

}
