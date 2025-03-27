using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryItem[] items;

    //define a delegate and event for item transactions
    public delegate void ItemTransactionHandler(InventoryItem item, string transactionType);
    public event ItemTransactionHandler OnItemTransaction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(InventoryItem item, int index)
    {
        // Add item to the inventory and trigger the OnItemTransaction event
        if(index >= 0 && index < items.Length)
        {
            items[index] = item;
            if(OnItemTransaction != null && item != null)
            {
                //Raise the event as a "buy" transaction
                OnItemTransaction.Invoke(item, "buy");
            }
        } 
            
    }

    public void RemoveItem(InventoryItem item, int index)
    {
        // Remove item from the inventory and trigger a sell event
        if(index >= 00 && index < items.Length)
        {
            items[index] = null;
            if(OnItemTransaction != null)
            {
                OnItemTransaction.Invoke(item, "sell");
            }
        }
    }
}
