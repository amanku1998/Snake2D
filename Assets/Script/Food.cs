using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    private FoodManager foodManager;
    private bool isMassGainer;

    private void Start()
    {
        StartCoroutine(foodManager.FoodLifeCycle());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);

        if (foodManager.currentCoroutine != null)
        {
            foodManager.StopFoodSpawnCoroutine();
        }

        foodManager.SpawnFoodRandomly();
    }

    public bool GetFoodType() { return isMassGainer; }
    public void SetFoodType(bool massGainer) { isMassGainer = massGainer; }
    public void SetFoodManager(FoodManager _foodManager) { foodManager = _foodManager; }
}
