using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject[] FoodPrefabs; // Mass Gainer and Mass Reducer prefabs
    [SerializeField] private Collider2D gridArea;

    //private SnakeController snake;
    [SerializeField] private SnakeController snake1;
    [SerializeField] private SnakeController snake2;

    [SerializeField] private float foodLifeSpan = 15f; // Time before food auto-destroys
    private GameObject currentFood; // Track the active food
    private int spawnCounter = 0; // Track the number of food spawns
    public Coroutine currentCoroutine;

    [Header("Power-Up Settings")]

    [SerializeField] private float powerUpSpawnIntervalMin = 15f;
    [SerializeField] private float powerUpSpawnIntervalMax = 25f;

    private GameObject currentPowerUp; // Track the active power-up
    [SerializeField] private GameObject[] powerUpPrefabs;

    [SerializeField] private float shieldDuration = 10f;
    [SerializeField] private float scoreBoostDuration = 10f;
    [SerializeField] private float speedBoostDuration = 10f;

    private ItemType activePowerUpType = ItemType.None; // Track the currently active power-up type
    private bool isPowerUpEffectActive = false; // Track if a power-up effect is active

    [SerializeField] private Image currentSelectedIcon;
    [SerializeField] private Sprite[] powerUpIcon;

    private float powerUpTimer;
    private float currentSpawnInterval;

    private void Start()
    {
        SpawnFoodRandomly();

        ResetPowerUpVariables();
        SetNewSpawnInterval();
    }

    private void Update()
    {
        if (isPowerUpEffectActive)
            return;

        powerUpTimer -= Time.deltaTime;

        if (powerUpTimer <= 0f)
        {
            SpawnNewPowerUp();
            SetNewSpawnInterval();
        }
    }


    public void SpawnFoodRandomly()
    {
        // Destroy existing food if it exists
        if (currentFood != null)
        {
            Destroy(currentFood);
        }

        Vector2 newPosition = GetRandomPositionForItem();

        bool isMassGainer = spawnCounter < 4 ? true : Random.Range(0, 10) < 8;
        // Select the appropriate prefab based on isMassGainer value
        GameObject selectedPrefab = isMassGainer ? FoodPrefabs[0] : FoodPrefabs[1];

        // Instantiate the selected prefab
        currentFood = Instantiate(selectedPrefab, newPosition, Quaternion.identity);

        // Assign the isMassGainer value to the instantiated food
        Food foodComponent = currentFood.GetComponent<Food>();
        // Assign the isMassGainer value to the instantiated food
        foodComponent.SetFoodType(isMassGainer);
        foodComponent.SetFoodManager(this);
    }

    //
    private void SpawnNewPowerUp()
    {
        // Destroy the existing power-up if it exists
        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
        }

        // Select a new random power-up and position
        GameObject newPowerUp = GetNewPowerUp();
        if (newPowerUp != null)
        {
            Vector2 spawnPosition = GetRandomPositionForItem();
            currentPowerUp = Instantiate(newPowerUp, spawnPosition, Quaternion.identity);
        }
    }

    private void SetNewSpawnInterval()
    {
        currentSpawnInterval = Random.Range(powerUpSpawnIntervalMin, powerUpSpawnIntervalMax);
        powerUpTimer = currentSpawnInterval;
    }

    public Vector2 GetRandomPositionForItem()
    {
        Bounds bounds = gridArea.bounds;
        Vector2 spawnPosition;

        do
        {
            int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
            int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
            spawnPosition = new Vector2(x, y);
        }//Check the power up is not get the position where any snake is already moving
        while (snake1.Occupies((int)spawnPosition.x, (int)spawnPosition.y) ||
            (GameModeManager.Instance.GetCurrentMode() == GameMode.Multiplayer &&
            snake2.Occupies((int)spawnPosition.x, (int)spawnPosition.y)));

        return spawnPosition;
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

    public void ReSpawnFood()
    {
        //
        SpawnFoodRandomly();
        // Start coroutine to handle food lifespan
        currentCoroutine = StartCoroutine(FoodLifeCycle());
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
        activePowerUpType = ItemType.None;
        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
        }
        DeactivateCurrentSelectedPowerIcon();
    }

    public void ApplyPowerUpEffect(ItemType powerUpType, SnakeController snake)
    {
        isPowerUpEffectActive = true;
        activePowerUpType = powerUpType;

        if (powerUpType == ItemType.Shield)
        {
            SetCurrentSelectedIcon((int)ItemType.Shield);
            StartCoroutine(ActivateShield(snake));
        }
        else if (powerUpType == ItemType.ScoreBooster)
        {
            SetCurrentSelectedIcon((int)ItemType.ScoreBooster);
            StartCoroutine(ActivateScoreBoost(snake));
        }
        else if (powerUpType == ItemType.SpeedUp)
        {
            SetCurrentSelectedIcon((int)ItemType.SpeedUp);
            StartCoroutine(ActivateSpeedBoost(snake));
        }
    }

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

    public void ClearCurrentPowerUp()
    {
        currentPowerUp = null; // Clear reference when collected
    }

    public int GetSpawnCounterVal() { return spawnCounter; }
    public void IncreaseSpawnCounterVal() { spawnCounter += 1; }
    public float GetShieldDuration() { return shieldDuration; }
    public float GetScoreBoostDuration() { return scoreBoostDuration; }
    public float GetSpeedBoostDuration() { return speedBoostDuration; }

    private IEnumerator ActivateShield(SnakeController snake)
    {
        snake.SetIsShieldActive(true);
        yield return new WaitForSeconds(GetShieldDuration());
        snake.SetIsShieldActive(false);
        ResetPowerUpVariables();
    }

    private IEnumerator ActivateScoreBoost(SnakeController snake)
    {
        snake.SetIsScoreBoostActive(true);
        yield return new WaitForSeconds(GetScoreBoostDuration());
        snake.SetIsScoreBoostActive(false);
        ResetPowerUpVariables();
    }

    private IEnumerator ActivateSpeedBoost(SnakeController snake)
    {
        snake.SetIsSpeedBoostActive(true);
        float curSnakeSpeed = snake.GetSpeed();
        snake.SetSpeed(curSnakeSpeed * 1.5f); // Adjust multiplier as needed

        yield return new WaitForSeconds(GetSpeedBoostDuration());

        snake.SetSpeed(snake.GetDefaultSpeed());
        snake.SetIsSpeedBoostActive(false);

        ResetPowerUpVariables();
    }
}
public enum ItemType { Shield , ScoreBooster , SpeedUp , None }

