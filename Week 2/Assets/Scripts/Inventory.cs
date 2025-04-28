using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryItem[] items;

    //define a delegate and event for item transactions
    public delegate void ItemTransactionHandler(InventoryItem item, string transactionType);
    public event ItemTransactionHandler OnItemTransaction;

    private Wallet playerWallet;

    private void Start()
    {
        playerWallet = FindObjectOfType<Wallet>();
        Debug.Log(playerWallet);
    }

    public void AddItem(InventoryItem item, int index)
    {
        // Add item to the inventory and trigger the OnItemTransaction event
        if(index >= 0 && index < items.Length)
        {
            //if there is an item already here which should be added
            if(items[index] != null)
            {
                RemoveItem(items[index], index);   
            }

            items[index] = item;
            if(OnItemTransaction != null && item != null)
            {
                if (playerWallet.Money < item.price)
                {
                    Debug.Log("cannot afford");
                    return;
                }
                //Raise the event as a "buy" transaction
                OnItemTransaction.Invoke(item, "buy");
                playerWallet.Money -= item.price;
            }
        } 
            
    }

    public void RemoveItem(InventoryItem item, int index)
    {
        // Remove item from the inventory and trigger a sell event
        if(index >= 0 && index < items.Length)
        {
            items[index] = null;
            if(OnItemTransaction != null)
            {
                OnItemTransaction.Invoke(item, "sell");
            }
        }
    }

    public void SaveInventory(string saveFilePath)
    {
        SerializableInventory serializableInventory = new SerializableInventory(this);
        string json = JsonUtility.ToJson(serializableInventory, true);
        System.IO.File.WriteAllText(saveFilePath, json);
        Debug.Log($"Inventory saved to {saveFilePath}");
    }

    public void LoadInventory(string saveFilePath)
    {
        if (System.IO.File.Exists(saveFilePath))
        {
            string json = System.IO.File.ReadAllText(saveFilePath);

            SerializableInventory serializableInventory =
                   JsonUtility.FromJson<SerializableInventory>(json);

            serializableInventory.ApplyToInventory(this);
            GameLogger.LogInfo($"Inventory loaded from {saveFilePath}", "File");
        }
        else
        {
            GameLogger.LogWarning($"Save file not found: {saveFilePath}", "File");
        }
    }
}

