using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    private FoodManager foodManager;
    private FoodManager1 foodManager1;
    private bool isMassGainer;

    private void Awake()
    {
        //Check if playing single mode
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            foodManager = FindObjectOfType<FoodManager>();
        }
        else //Check if playing Multiplayer mode
        {
            foodManager1 = FindObjectOfType<FoodManager1>();
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (foodManager != null)
            {
                StartCoroutine(foodManager.FoodLifeCycle());
            }
        }
        else
        {
            if (foodManager1 != null)
            {
                StartCoroutine(foodManager1.FoodLifeCycle());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
        //Check if playing single mode
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (foodManager.currentCoroutine != null)
            {
                foodManager.StopFoodSpawnCoroutine();
            }
            foodManager.SpawnFoodRandomly();
        }
        else
        {
            if (foodManager1.currentCoroutine != null)
            {
                foodManager1.StopFoodSpawnCoroutine();
            }

            foodManager1.SpawnFoodRandomly();
        }
    }

    public bool GetFoodType() { return isMassGainer; }
    public void SetFoodType(bool massGainer) { isMassGainer = massGainer; }
}
