using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryItem[] items;

    public DraggableItem itemPrefab;
    public InventorySlot slotPrefab;

    public InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        //create a slot
        slots = new InventorySlot[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            slots[i] = Instantiate(slotPrefab, transform);

            DraggableItem draggableItem = Instantiate(itemPrefab, slots[i].transform);
            //set the draggable item's data according to the inventory item array
            draggableItem.SetItem(items[i]);
            
            //assign this new draggable item to the slot
            slots[i].Init(this, i, draggableItem);
        }
    }
}
