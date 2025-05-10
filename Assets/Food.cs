using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Collider2D gridArea;
    private SnakeController snake;

    private void Awake()
    {
        snake = FindObjectOfType<SnakeController>();
    }

    private void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        Vector2 newPosition;

        do
        {
            // Generate random positions within bounds
            int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
            int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
            newPosition = new Vector2(x, y);
        }
        while (snake.Occupies((int)newPosition.x, (int)newPosition.y));

        // Assign the position
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();
    }
}
