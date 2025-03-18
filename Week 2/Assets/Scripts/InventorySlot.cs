using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InventoryUI parentInventory;
    public int index;
    public DraggableItem draggableItem;

    public void Init(InventoryUI par, int ind, DraggableItem it)
    {
        index = ind;
        parentInventory = par;
        draggableItem = it;
        draggableItem.slot = this;
    }

    public void UpdateItem(InventoryItem item)
    {
        // update the backend array
        parentInventory.items[index] = item;

        // update the front end array
        draggableItem.SetItem(item);
        
    }

}
