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

    [Header("Slime Data")] public GameObject slimePrefab;
    public uint slimeStart = 100;
    public float slimeSpawnDelay = 5;
    private List<Slime> activeSlimes;
    private ObjectPool<Slime> slimePool;

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
        
        //slime init
        activeSlimes = new List<Slime>();
        slimePool = new ObjectPool<Slime>(
            OnCreateSlime,
            OnPopSlime,
            OnReturnSlime,
            OnDestroySlime,
            slimeStart);
        for (int i = 0; i < slimeStart; i++)
        {
            slimePool.PopFromPool();
        }

        StartCoroutine(TriggerAfterDelayLoop(slimeSpawnDelay, () => slimePool.PopFromPool()));
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

    private Slime OnCreateSlime()
    {
        return Instantiate(slimePrefab, transform).GetComponent<Slime>();
    }

    private void OnPopSlime(Slime slime)
    {
        slime.gameObject.SetActive(true);
        slime.transform.position = new Vector3(
            Random.Range(-spawnArea.x * 0.5f, spawnArea.x * 0.5f),
            0,
            Random.Range(-spawnArea.y * 0.5f, spawnArea.y * 0.5f)
        );
        slime.Init(this);
        activeSlimes.Add(slime);
    }

    private void OnReturnSlime(Slime slime)
    {
        slime.gameObject.SetActive(false);
        activeSlimes.Remove(slime);
    }

    private void OnDestroySlime(Slime slime)
    {
        Destroy(slime.gameObject);
    }

    public void RemoveSlime(Slime slime)
    {
        slimePool.ReturnToPool(slime);
    }
}
