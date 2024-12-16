using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController1 : MonoBehaviour
{
    public Transform targetGameObject; // The GameObject whose position you want to assign to the snake
    public string playerID; // "Player1" or "Player2"
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private int initialSize = 4;

    [Header("Game Objects")]
    [SerializeField] private Transform segmentPrefab;

    [Header("Game Settings")]
    [SerializeField] private bool moveThroughWalls = false;

    [SerializeField] private Vector2Int direction = Vector2Int.right;
    private List<Transform> segmentOfSnakeBodyPartList = new List<Transform>();
    private Vector2Int input = Vector2Int.zero;
    private float nextUpdate;

    [SerializeField] private FoodManager1 foodManager;
    private float defaultSpeedMultiplier;
    [SerializeField] private KeyCode upKey, downKey, leftKey, rightKey; // Controls for the player

    private void Awake()
    {
        ResetState();
        defaultSpeedMultiplier = speed;
    }

    private void Start()
    {
        ////Move the snake in the direction it is facing if no target is set
        int x = Mathf.RoundToInt(targetGameObject.position.x);
        int y = Mathf.RoundToInt(targetGameObject.position.y);
        transform.position = new Vector2(x, y);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Prevent turning back on itself
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(upKey)) { input = Vector2Int.up; }
            else if (Input.GetKeyDown(downKey)) { input = Vector2Int.down; }
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(leftKey)) { input = Vector2Int.left; }
            else if (Input.GetKeyDown(rightKey)) { input = Vector2Int.right; }
        }

        // Update direction
        if (input != Vector2Int.zero)
        {
            direction = input;
        }
    }

    private void FixedUpdate()
    {
        // Wait until the next update before proceeding
        if (Time.time < nextUpdate)
        {
            return;
        }
        // Set the new direction based on the input
        if (input != Vector2Int.zero)
        {
            direction = input;
        }
        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = segmentOfSnakeBodyPartList.Count - 1; i > 0; i--)
        {
            segmentOfSnakeBodyPartList[i].position = segmentOfSnakeBodyPartList[i - 1].position;
        }
        //// Round the values to ensure it aligns to the grid
        // Move the snake in the direction it is facing if no target is set
        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;
        transform.position = new Vector2(x, y);

        // Set the next update time based on the speed
        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segmentOfSnakeBodyPartList[segmentOfSnakeBodyPartList.Count - 1].position;
        segmentOfSnakeBodyPartList.Add(segment);
    }

    public void Shrink(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (segmentOfSnakeBodyPartList.Count > initialSize)
            {
                Transform lastSegment = segmentOfSnakeBodyPartList[segmentOfSnakeBodyPartList.Count - 1];
                Destroy(lastSegment.gameObject);
                segmentOfSnakeBodyPartList.Remove(lastSegment);               
            }
        }
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        //transform.position = Vector3.zero;
        transform.position = targetGameObject.position;
        // Start at 1 to skip destroying the head
        for (int i = 1; i < segmentOfSnakeBodyPartList.Count; i++)
        {
            Destroy(segmentOfSnakeBodyPartList[i].gameObject);
        }
        // Clear the list but add back this as the head
        segmentOfSnakeBodyPartList.Clear();
        segmentOfSnakeBodyPartList.Add(transform);

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }

    //
    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in segmentOfSnakeBodyPartList)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y)
            {
                return true;
            }
        }

        return false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Food food = other.GetComponent<Food>();
            if (food != null)
            {
                //
                ScoreManager1 scoreManager = ScoreManager1.Instance;
                //Check if food is mass gainer
                if (food.GetFoodType() == true)
                {
                    Debug.Log("isScoreBoostActive :"+ foodManager.GetIsScoreBoostActive());
                    //Check if snake get double score
                    if (foodManager.GetIsScoreBoostActive())
                    {
                        //Increase score at double rate
                        int increasedScore = scoreManager.GetScoreVal(); 
                        int scoreMultiplier = scoreManager.GetIncreamentScoreMultiplierVal(); 
                        scoreManager.AddScore(increasedScore * scoreMultiplier , this);
                    }
                    else
                    {
                        int scoreVal = scoreManager.GetScoreVal();
                        Debug.Log("ScoreVal :" + scoreVal);
                        scoreManager.AddScore(scoreVal, this);
                    }

                    foodManager.IncreaseSpawnCounterVal();
                    Grow();
                }
                else if (food.GetFoodType() == false)   ////Check if food is mass burner
                {
                    scoreManager.ReduceScore(scoreManager.GetScoreVal(), this);
                    
                    Shrink(1);
                }
            }
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            PowerUpController powerUp = other.GetComponent<PowerUpController>();
            if (powerUp != null)
            {
                // Prevent duplicate calls using a flag
                if (!powerUp.HasBeenActivated)
                {
                    powerUp.HasBeenActivated = true; // Mark as processed
                    foodManager.ApplyPowerUpEffect(powerUp.powerUpType, this);
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.gameObject.CompareTag("Obstacle") )
        {
            if (!foodManager.GetIsShieldActive()){
                GameManagerMultiplayer.Instance.DisplayGameOverPanel();
            }
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls)
            {
                Traverse(other.transform);
            }
            else if (!foodManager.GetIsShieldActive())
            {
                GameManagerMultiplayer.Instance.DisplayGameOverPanel();
            }
        }
    }

    private void Traverse(Transform wall)
    {
        Vector3 position = transform.position;
        if (direction.x != 0f)
        {
            position.x = Mathf.RoundToInt(-wall.position.x + direction.x);
        }
        else if (direction.y != 0f)
        {
            position.y = Mathf.RoundToInt(-wall.position.y + direction.y);
        }
        transform.position = position;
    }

    public void SetSpeed(float _speed) { speed = _speed; }
    public float GetSpeed() { return speed; }
    public float GetSnakeDefaultSize() { return initialSize; }
    public float GetDefaultSpeed() { return defaultSpeedMultiplier; }
    public List<Transform> GetSegmentOfSnakeBodyPartList() { return segmentOfSnakeBodyPartList; }
}
