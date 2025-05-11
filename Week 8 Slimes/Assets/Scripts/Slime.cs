using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour
{
    public enum SlimeType
    {
        Player,
        Passive,
        Chaser,
        Coward,
        MaxSize
    }

    public int currentSize;
    public float currentSpeed;
    public SlimeType slimeType;

    protected float aiUpdateDelay = 0.1f;
    protected float decreaseSizeDelay = 2.5f;
    protected GameObject targetObject;
    protected GameManager gameManager;

    protected void Start()
    {
        StartCoroutine(TriggerCheckLoop(decreaseSizeDelay, () => ChangeSize(-1)));
    }

    public virtual void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;
        slimeType = (SlimeType)Random.Range(1, (int)SlimeType.MaxSize);
        foreach (Renderer rendererChild in GetComponentsInChildren<Renderer>())
        {
            rendererChild.material.color = gameManager.slimeColours[(int)slimeType];
        }
        //GetComponentInChildren<MeshRenderer>().material.color 
    }

    protected IEnumerator TriggerCheckLoop(float delay, System.Action action)
    {
        action?.Invoke();
        yield return new WaitForSeconds(Random.Range(0.0f, delay));
        while (true)
        {
            action?.Invoke();
            yield return new WaitForSeconds(delay);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    protected virtual void Update()
    {
        //moves player towards target
        transform.position = Vector3.MoveTowards(
            transform.position,
            //sets target position to player position if target doesn't exist 
            (targetObject ? targetObject.transform.position : transform.position),
            currentSpeed * Time.deltaTime
        );
        
        //clamp player position
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -gameManager.spawnArea.x * 0.5f, gameManager.spawnArea.x * 0.5f),
            0,
            Mathf.Clamp(transform.position.z, -gameManager.spawnArea.y * 0.5f, gameManager.spawnArea.y * 0.5f)
        );
    }

    private void EatSlime(Slime otherSlime)
    {
        if (currentSize > otherSlime.currentSize)
        {
            ChangeSize(otherSlime.currentSize);
            otherSlime.ChangeSize(-otherSlime.currentSize);
        }
        else if (currentSize < otherSlime.currentSize)
        {
            otherSlime.ChangeSize(currentSize);
            ChangeSize(-currentSize);
        }
    }

    protected virtual void ChangeSize(int deltaSize)
    {
        currentSize = Mathf.Min(currentSize + deltaSize, 100);
        if (currentSize <= 0)
        {
            gameManager.RemoveSlime(this);
            return;
        }

        transform.localScale = Vector3.one * (2 * Mathf.Log(2 * currentSize, 2) - 1);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Food>(out Food food))
        {
            ChangeSize(food.growth);
            gameManager.RemoveFood(food);
        }
        
        if (other.transform.TryGetComponent<Slime>(out Slime slime))
        {
            EatSlime(slime);
        }
    }
}
