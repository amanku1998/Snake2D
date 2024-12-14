using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject[] FoodPrefabs; // Mass Gainer and Mass Reducer prefabs
    [SerializeField] private Collider2D gridArea;

    private SnakeController snake;

    public float foodLifeSpan = 15f; // Time before food auto-destroys
    public GameObject currentFood; // Track the active food
    private int spawnCounter = 0; // Track the number of food spawns

    [Header("Power-Up Settings")]
    [SerializeField] private float powerUpSpawnIntervalMin = 15f;
    [SerializeField] private float powerUpSpawnIntervalMax = 25f;

    public GameObject currentPowerUp; // Track the active power-up
    //private bool lastFoodWasMassGainer = true; // Track the type of the last food
    [SerializeField] private GameObject[] powerUpPrefabs;


    public Coroutine currentCoroutine;

    private string activePowerUpType = ""; // Track the currently active power-up type
    public bool isPowerUpEffectActive = false; // Track if a power-up effect is active

    [Header("Power-Ups")]
    [SerializeField] private float shieldDuration = 3f;
    [SerializeField] private float scoreBoostDuration = 3f;
    [SerializeField] private float speedBoostDuration = 10f;

    private bool isShieldActive = false;
    private bool isScoreBoostActive = false;
    private bool isSpeedBoostActive = false;

    private void Awake()
    {
        snake = FindObjectOfType<SnakeController>();
    }

    private void Start()
    {
        SpawnFoodRandomly();

        StartCoroutine(SpawnPowerUps());
    }

    public void ReSpawnFood()
    {
        //
        SpawnFoodRandomly();
        // Start coroutine to handle food lifespan
        currentCoroutine = StartCoroutine(FoodLifeCycle());
    }

    public void SpawnFoodRandomly()
    {
        // Destroy existing food if it exists
        if (currentFood != null)
        {
            Destroy(currentFood);
        }

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

        bool isMassGainer;

        if (spawnCounter < 3)
        {
            // Ensure the first three spawns are always Mass Gainers
            isMassGainer = true;
        }
        else
        {
            // Determine isMassGainer value randomly
            // Determine isMassGainer value with weighted probability
            //bool randomIsMassGainer = Random.Range(0, 10) < 8; // 70% chance for true (mass gainer), 20% chance for false (mass burner)
            isMassGainer = Random.Range(0, 10) < 8; // 70% chance for true (mass gainer), 20% chance for false (mass burner)

        }
        // Select the appropriate prefab based on isMassGainer value
        //GameObject selectedPrefab = randomIsMassGainer ? FoodPrefabs[0] : FoodPrefabs[1];
        GameObject selectedPrefab = isMassGainer ? FoodPrefabs[0] : FoodPrefabs[1];

        // Instantiate the selected prefab
        currentFood = Instantiate(selectedPrefab, newPosition, Quaternion.identity);

        // Assign the isMassGainer value to the instantiated food
        Food foodComponent = currentFood.GetComponent<Food>();
        // Assign the isMassGainer value to the instantiated food
        foodComponent.SetFoodType(isMassGainer);
    }

    public void StopFoodSpawnCoroutine()
    {
        StopCoroutine(FoodLifeCycle());
    }

    public IEnumerator FoodLifeCycle()
    {
        yield return new WaitForSeconds(foodLifeSpan);

        // If the food was not collected, spawn a new random food
        if (currentFood != null)
        {
            Destroy(currentFood);
            ReSpawnFood();
        }
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            // Wait until no power-up effect is active
            yield return new WaitUntil(() => !isPowerUpEffectActive);

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

            // Select a new power-up that the snake doesn't already have
            GameObject newPowerUp = GetNewPowerUp();
            Debug.Log("newPowerUp :"+ newPowerUp.name);
            if (newPowerUp != null && isPowerUpEffectActive == false)
            {
                currentPowerUp = Instantiate(newPowerUp, spawnPosition, Quaternion.identity);
            }
        }
    }

    private GameObject GetNewPowerUp()
    {
        List<GameObject> availablePowerUps = new List<GameObject>();

        foreach (var powerUp in powerUpPrefabs)
        {
            PowerUpController powerUpComponent = powerUp.GetComponent<PowerUpController>();
            if (powerUpComponent != null && powerUpComponent.powerUpType != activePowerUpType)
            {
                availablePowerUps.Add(powerUp);
            }
        }

        if (availablePowerUps.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePowerUps.Count);
            return availablePowerUps[randomIndex];
        }

        return null; // No valid power-up to spawn
    }

    private void ResetPowerUpVariables()
    {
        isPowerUpEffectActive = false;
        activePowerUpType = ""; // Clear the active power-up type
        ClearCurrentPowerUp();
    }

    public void ApplyPowerUpEffect(string powerUpType)
    {
        activePowerUpType = powerUpType;
        isPowerUpEffectActive = true;

        Debug.Log("Call ActivatePowerUp :" + powerUpType.ToString());
        switch (powerUpType.ToString())
        {
            case "Shield":
                Debug.Log("StartCoroutine Shield");
                StartCoroutine(ActivateShield());
                break;

            case "ScoreBoost":
                Debug.Log("StartCoroutine Score Boost");
                StartCoroutine(ActivateScoreBoost());
                break;

            case "SpeedUp":
                Debug.Log("StartCoroutine Speed Boost");
                StartCoroutine(ActivateSpeedBoost());
                break;
        }
    }

    public void ClearCurrentPowerUp()
    {
        currentPowerUp = null; // Clear reference when collected
    }

    public float GetShieldDuration()    {   return shieldDuration;  }

    public float GetScoreBoostDuration()    {   return scoreBoostDuration;  }

    public float GetSpeedBoostDuration()    {   return speedBoostDuration;  }

    public bool GetIsShieldActive() { return isShieldActive; }

    public bool GetIsScoreBoostActive() { return isScoreBoostActive; }

    public bool GetIsSpeedBoostActive() { return isSpeedBoostActive; }

    private IEnumerator ActivateShield()
    {
        isShieldActive = true;
        yield return new WaitForSeconds(GetShieldDuration());
        isShieldActive = false;

        ResetPowerUpVariables();
    }

    private IEnumerator ActivateScoreBoost()
    {
        isScoreBoostActive = true;
        yield return new WaitForSeconds(GetScoreBoostDuration());
        isScoreBoostActive = false;

        ResetPowerUpVariables();
    }

    private IEnumerator ActivateSpeedBoost()
    {
        isSpeedBoostActive = true;
        float curSnakeSpeed = snake.GetSpeed();
        snake.SetSpeed(curSnakeSpeed * 1.5f); // Adjust multiplier as needed
        Debug.Log("Increase speed :" + snake.GetSpeed());
        yield return new WaitForSeconds(GetSpeedBoostDuration());
        snake.SetSpeed(snake.GetDefaultSpeed());
        isSpeedBoostActive = false;

        ResetPowerUpVariables();
    }

}
