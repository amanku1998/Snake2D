using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private Vector2Int direction = Vector2Int.right;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private int initialSize = 4;
    [SerializeField] private bool moveThroughWalls = false;

    private List<Transform> segmentOfSnakeBodyPartList = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;

    [SerializeField] private FoodManager foodManager;

    private float defaultSpeedMultiplier;

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
    
    public float GetSpeed()
    {
        return speed;
    }

    public float GetDefaultSpeed()
    {
        return defaultSpeedMultiplier;
    }

    public List<Transform> GetSegmentOfSnakeBodyPartList()
    {
        return segmentOfSnakeBodyPartList;
    }

    private void Start()
    {
        ResetState();
        defaultSpeedMultiplier = speed;
    }

    private void Update()
    {
        // Only allow turning up or down while moving in the x-axis
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                input = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                input = Vector2Int.down;
            }      
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Shrink(1);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                ScoreManager.Instance.AddScore(10);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ScoreManager.Instance.ReduceScore(10);
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                input = Vector2Int.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = Vector2Int.left;
            }
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
        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
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
                segmentOfSnakeBodyPartList.Remove(lastSegment);
                Destroy(lastSegment.gameObject);
            }
        }
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        transform.position = Vector3.zero;
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
            //Grow();
            //Food food = other.GetComponent<Food>();
            Food food = other.GetComponent<Food>();

            if (food != null)
            {
                ScoreManager scoreManager = ScoreManager.Instance;

                if (food.GetFoodType() == true)
                {
                    Debug.Log("isScoreBoostActive :"+ foodManager.GetIsScoreBoostActive());
                    if (foodManager.GetIsScoreBoostActive())
                    {
                        //Increase score at double rate
                        int increasedScore = scoreManager.GetScoreVal(); 
                        int scoreMultiplier = scoreManager.GetIncreamentScoreMultiplierVal(); 
                        scoreManager.AddScore(increasedScore * scoreMultiplier);
                    }
                    else
                    {
                        int scoreVal = scoreManager.GetScoreVal();
                        Debug.Log("ScoreVal :" + scoreVal);
                        scoreManager.AddScore(scoreVal);
                    }
                    
                    Grow();
                }
                else if (food.GetFoodType() == false)
                {
                    scoreManager.ReduceScore(scoreManager.GetScoreVal());
                    //Shrink(food.lengthChangeAmount);
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
                    Debug.Log("powerUpType :" + powerUp.powerUpType.ToString());
                    //To do it is called twice which causes issue
                    //ActivatePowerUp(powerUp.powerUpType.ToString());
                    foodManager.ApplyPowerUpEffect(powerUp.powerUpType);
                    //foodManager.ActivatePowerUp(powerUp.powerUpType);

                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!foodManager.GetIsShieldActive())
            {
                ResetState();
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
                ResetState();
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
}
