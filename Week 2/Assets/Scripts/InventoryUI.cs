using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;

    public DraggableItem itemPrefab;
    public InventorySlot slotPrefab;

    public InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        inventory.OnItemTransaction += HandleItemTransaction;
    }

    public void Init()
    {
        //create a slot
        slots = new InventorySlot[inventory.items.Length];
        for (int i = 0; i < inventory.items.Length; i++)
        {
            slots[i] = Instantiate(slotPrefab, transform);

            DraggableItem draggableItem = Instantiate(itemPrefab, slots[i].transform);
            //set the draggable item's data according to the inventory item array
            draggableItem.SetItem(inventory.items[i]);
            
            //assign this new draggable item to the slot
            slots[i].Init(this, i, draggableItem);
        }
    }

    private void HandleItemTransaction(InventoryItem item, string transactionType)
    {
        if(transactionType == "buy")
        {
            Debug.Log($"Item bought: {item.name} by {gameObject.name}");
            // Additional UI update for buying an item
        }
        else if(transactionType == "sell")
        {
            Debug.Log($"Item sold: {item.name} by {gameObject.name}");
        }
        else
        {
            Debug.LogError("Whoopsie! You've got an incorrect transaction type in HandleItemTransaction!");
        }
    }

    public void Save(string saveFilePath)
    {
        inventory.SaveInventory(saveFilePath);
    }

    public void Load(string saveFilePath)
    {
        inventory.LoadInventory(saveFilePath);
        ReInitialise();
    }

    // called after loading a saved inventory
    public void ReInitialise()
    {
        // destroy all draggable items in the slots
        foreach (InventorySlot slot in slots)
        {
            Destroy(slot.draggableItem.gameObject);
            slot.draggableItem = null;
        }

        // instantiate new draggable items from the inventory items 
        // (loaded from the save file)
        for (int i = 0; i < inventory.items.Length; i++)
        {
            DraggableItem draggableItem = Instantiate(itemPrefab, slots[i].transform);
            draggableItem.SetItem(inventory.items[i]);
            slots[i].Init(this, i, draggableItem);
        }
    }

}
