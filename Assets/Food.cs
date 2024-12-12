using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    [SerializeField] private Collider2D gridArea;

    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private Sprite[] MassGainerImage;

    private SnakeController snake;

    public bool isMassGainer = true; // true for Mass Gainer, false for Mass Burner
    public int lengthChangeAmount = 1; // Units to grow or shrink
    public float lifeSpan = 15f; // Time before food auto-destroys

    [Header("Power-Up Settings")]

    [SerializeField] private float powerUpSpawnIntervalMin = 5f;
    [SerializeField] private float powerUpSpawnIntervalMax = 10f;

    private GameObject currentPowerUp; // Track the active power-up
    private Coroutine autoSpawnCoroutine;

    private bool isCoroutineRunning = false;

    private void Awake()
    {
        snake = FindObjectOfType<SnakeController>();
    }

    private void Start()
    {
        RandomizePosition();
        //StartCoroutine(AutoSpawn());
        StartCoroutine(SpawnPowerUps());
    }

    public Sprite[] GetCurrentFoodImage()
    {
        return MassGainerImage;
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

        //int randomValue = Random.Range(1, 101); // Upper limit is exclusive, so use 101
        //Debug.Log("Random Value : " + randomValue);
        //// Assign Mass Gainer or Mass Burner
        //isMassGainer = (randomValue < 50) ? true : false;

        //gameObject.GetComponent<SpriteRenderer>().sprite = (isMassGainer) ? MassGainerImage[0] : MassGainerImage[1];

        // Assign the position
        transform.position = newPosition;

        if (!isCoroutineRunning)
        {
            StartCoroutine(AutoSpawn());
        }
    }

    private IEnumerator AutoSpawn()
    {
        isCoroutineRunning = true;

        //while (true) // Continuous loop
        //{
            yield return new WaitForSeconds(lifeSpan); // Wait for the lifespan
            RandomizePosition(); // Generate a new position for the food

            isCoroutineRunning = false;
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(powerUpSpawnIntervalMin, powerUpSpawnIntervalMax));

            // Destroy existing power-up if it exists
            if (currentPowerUp != null)
            {
                Destroy(currentPowerUp);
            }

            Bounds bounds = gridArea.bounds;
            Vector2 spawnPosition;

            do
            {
                int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
                int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
                spawnPosition = new Vector2(x, y);
            }
            while (snake.Occupies((int)spawnPosition.x, (int)spawnPosition.y));

            // Spawn a new power-up and keep track of it
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            currentPowerUp  = Instantiate(powerUpPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }

    public void ClearCurrentPowerUp()
    {
        currentPowerUp = null; // Clear reference when collected
    }

}
