using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InfoBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameOfItem;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI price;

    //how high up the pop up appears from the item
    [SerializeField] private float offset;

    private Canvas canvas;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    //sets all parameters
    public void SetContents(InventoryItem item)
    {
        SetNameOfItem(item.itemName);
        SetDescription(item.description);
        SetPrice(item.price, item);
    }
    
    // sets the name of the item for the popup box
    public void SetNameOfItem(string inputName)
    {
        nameOfItem.text = inputName;
    }
    
    // sets the description of the item for the popup box
    public void SetDescription(string inputName)
    {
        description.text = inputName;
    }
    
    // sets the description of the item for the popup box
    public void SetPrice(int inputPrice)
    {
        string priceAsString = inputPrice.ToString();

        price.text = "$ " + priceAsString;
        
    }

    public void SetPrice(int inputPrice, InventoryItem item)
    {
        SetPrice(inputPrice);
        
        Wallet playerWallet = FindObjectOfType<Wallet>();
        
        //checks if the player the amount required
        if (playerWallet.Money >= inputPrice)
        {
            price.color = Color.green;
        }
        else
        {
            price.color = Color.red;
        }
    }

    public void Summon(Vector3 positionToGoTo)
    {
        Vector3 targetPosition = positionToGoTo;

        targetPosition = new Vector3(
            targetPosition.x,
            ((Screen.height / 1080.0f) * offset) + targetPosition.y,
            targetPosition.z
            );
        
        transform.position = targetPosition;
    }

    public void Summon(Vector3 positionToGoTo, InventoryItem item)
    {
        Summon(positionToGoTo);
        SetContents(item);
    }

    public void Vanish()
    {
        transform.position = new Vector3(999999, 999999, 999999);
    }
}
