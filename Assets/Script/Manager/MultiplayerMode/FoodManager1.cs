using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FoodManager1 : MonoBehaviour
{
    [SerializeField] private GameObject[] FoodPrefabs; // Mass Gainer and Mass Reducer prefabs
    [SerializeField] private Collider2D gridArea;

    //private SnakeController snake;
    private SnakeController1 snake1;
    private SnakeController1 snake2;

    [SerializeField] private float foodLifeSpan = 15f; // Time before food auto-destroys
    private GameObject currentFood; // Track the active food
    private int spawnCounter = 0; // Track the number of food spawns
    public Coroutine currentCoroutine;

    [Header("Power-Up Settings")]

    [SerializeField] private float powerUpSpawnIntervalMin = 15f;
    [SerializeField] private float powerUpSpawnIntervalMax = 25f;

    private GameObject currentPowerUp; // Track the active power-up
    [SerializeField] private GameObject[] powerUpPrefabs;

    [SerializeField] private float shieldDuration = 3f;
    [SerializeField] private float scoreBoostDuration = 3f;
    [SerializeField] private float speedBoostDuration = 10f;

    private bool isShieldActive = false;
    private bool isScoreBoostActive = false;
    private bool isSpeedBoostActive = false;

    private string activePowerUpType = ""; // Track the currently active power-up type
    private bool isPowerUpEffectActive = false; // Track if a power-up effect is active

    [SerializeField] private Image currentSelectedIcon;
    [SerializeField] private Sprite[] powerUpIcon;

    public void SetCurrentSelectedIcon(int powerUpIndex)
    {
        currentSelectedIcon.enabled = true;
        currentSelectedIcon.sprite = powerUpIcon[powerUpIndex];
    }

    public void DeactivateCurrentSelectedPowerIcon()
    {
        currentSelectedIcon.enabled = false;
        currentSelectedIcon.sprite = null;
    }

    private void Awake()
    {
        snake1 = GameObject.FindWithTag("Snake1").GetComponent<SnakeController1>();
        snake2 = GameObject.FindWithTag("Snake2").GetComponent<SnakeController1>();
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
        while (snake1.Occupies((int)newPosition.x, (int)newPosition.y) || snake2.Occupies((int)newPosition.x, (int)newPosition.y));

        bool isMassGainer = spawnCounter < 4 ? true : Random.Range(0, 10) < 8;
        // Select the appropriate prefab based on isMassGainer value
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
            while (snake1.Occupies((int)spawnPosition.x, (int)spawnPosition.y) || snake2.Occupies((int)spawnPosition.x, (int)spawnPosition.y));

            // Select a new power-up that the snake doesn't already have
            GameObject newPowerUp = GetNewPowerUp();
            Debug.Log("newPowerUp :" + newPowerUp.name);
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

    public void ApplyPowerUpEffect(string powerUpType, SnakeController1 snake)
    {
        isPowerUpEffectActive = true;
        activePowerUpType = powerUpType;

        if (powerUpType == "Shield")
        {
            SetCurrentSelectedIcon((int)ItemType.Shield);
            StartCoroutine(ActivateShield(snake));
        }
        else if (powerUpType == "ScoreBoost")
        {
            SetCurrentSelectedIcon((int)ItemType.ScoreBooster);
            StartCoroutine(ActivateScoreBoost(snake));
        }
        else if (powerUpType == "SpeedUp")
        {
            SetCurrentSelectedIcon((int)ItemType.SpeedUp);
            StartCoroutine(ActivateSpeedBoost(snake));
        }
    }

    public void ClearCurrentPowerUp()
    {
        currentPowerUp = null; // Clear reference when collected
    }

    public int GetSpawnCounterVal() { return spawnCounter; }
    public void IncreaseSpawnCounterVal() { spawnCounter += 1; }
    public float GetShieldDuration() { return shieldDuration; }
    public float GetScoreBoostDuration() { return scoreBoostDuration; }
    public float GetSpeedBoostDuration() { return speedBoostDuration; }
    public bool GetIsShieldActive() { return isShieldActive; }
    public bool GetIsScoreBoostActive() { return isScoreBoostActive; }
    public bool GetIsSpeedBoostActive() { return isSpeedBoostActive; }

    private IEnumerator ActivateShield(SnakeController1 snake)
    {
        if (snake.tag == "Snake1" || snake.tag == "Snake2")
        {
            isShieldActive = true;   // Add method in SnakeController1 to handle shield
        }

        yield return new WaitForSeconds(GetShieldDuration());

        if (snake.tag == "Snake1" || snake.tag == "Snake2")
        {
            isShieldActive = false;  // Deactivate shield
        }

        ResetPowerUpVariables();
    }

    private IEnumerator ActivateScoreBoost(SnakeController1 snake)
    {
        if (snake.tag == "Snake1" || snake.tag == "Snake2")
        {
            isScoreBoostActive = true;
        }

        yield return new WaitForSeconds(GetScoreBoostDuration());

        if (snake.tag == "Snake1" || snake.tag == "Snake2")
        {
            isScoreBoostActive = false;
        }

        ResetPowerUpVariables();
    }

    private IEnumerator ActivateSpeedBoost(SnakeController1 snake)
    {
        if (snake.tag == "Snake1" || snake.tag == "Snake2")
        {
            isSpeedBoostActive = true;
            float curSnakeSpeed = snake.GetSpeed();
            snake.SetSpeed(curSnakeSpeed * 1.5f); // Adjust multiplier as needed

            Debug.Log("Increase speed :" + snake.GetSpeed());
        }

        yield return new WaitForSeconds(GetSpeedBoostDuration());

        if (snake.tag == "Snake1" || snake.tag == "Snake2")
        {
            snake.SetSpeed(snake.GetDefaultSpeed());
            isSpeedBoostActive = false;
        }

        ResetPowerUpVariables();
    }

}
