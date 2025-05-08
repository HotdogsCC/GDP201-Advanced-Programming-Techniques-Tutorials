using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector2 spawnArea = new Vector2(100, 100);
    public List<Color> slimeColours;

    [Header("Food Data")]
    public GameObject foodPrefab;
    public uint foodStart = 100;
    public float foodSpawnDelay = 2.5f;
    private List<Food> activeFoods;
    private ObjectPool<Food> foodPool;

    // Start is called before the first frame update
    void Start()
    {
        //Food init
        activeFoods = new List<Food>();
        foodPool = new ObjectPool<Food>(
            OnCreateFood,
            OnPopFood,
            OnReturnFood,
            OnDestoryFood,
            foodStart
            );
        for(int i = 0; i < foodStart; i++)
        {
            foodPool.PopFromPool();
        }
        StartCoroutine(TriggerAfterDelayLoop(foodSpawnDelay, () => foodPool.PopFromPool()));
    }

    private IEnumerator TriggerAfterDelayLoop(float delay, System.Action action)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke(); //checks action isnt null and then does the thing
        }
    }

    private Food OnCreateFood()
    {
        return Instantiate(foodPrefab, transform).GetComponent<Food>();
    }

    private void OnPopFood(Food food)
    {
        food.gameObject.SetActive(true);
        food.transform.position = new Vector3(
            Random.Range(-spawnArea.x * 0.5f, spawnArea.x * 0.5f),
            0,
            Random.Range(-spawnArea.y * 0.5f, spawnArea.y * 0.5f)
        );
        activeFoods.Add(food);
    }

    private void OnReturnFood(Food food)
    {
        food.gameObject.SetActive(false);
        activeFoods.Remove(food);
    }

    private void OnDestoryFood(Food food)
    {
        Destroy(food.gameObject);
    }

    public void RemoveFood(Food food)
    {
        foodPool.ReturnToPool(food);
    }
}
