using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    private FoodManager foodManager;
    private bool isMassGainer;

    private void Awake()
    {
        //snake = FindObjectOfType<SnakeController>();
        foodManager = FindObjectOfType<FoodManager>();
    }

    private void Start()
    {
        if (foodManager != null)
        {
            StartCoroutine(foodManager.FoodLifeCycle());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
        //RandomizePosition();
        if (foodManager.currentCoroutine != null)
        {
            foodManager.StopFoodSpawnCoroutine();
        }
        foodManager.SpawnFoodRandomly();
    }

    public bool GetFoodType() { return isMassGainer; }
    public void SetFoodType(bool massGainer) { isMassGainer = massGainer; }
}
