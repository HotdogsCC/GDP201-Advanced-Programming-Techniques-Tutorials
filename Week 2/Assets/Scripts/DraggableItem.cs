using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public bool dragging;
    public Transform originalParent;
    public Canvas canvas;

    public InventorySlot slot;

    public Image image;

    public InventoryItem item;
    public void SetItem(InventoryItem i)
    {
        item = i;
        if (image)
        {
            if (item != null)
            {
                image.sprite = item.icon;
                image.color = item.color;
            }
            gameObject.SetActive(item != null);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (originalParent == null)
        {
            originalParent = transform.parent;
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragging)
        {
            transform.position = eventData.position;
        }
    }

    List<RaycastResult> hits = new List<RaycastResult>();

    public void OnEndDrag(PointerEventData eventData)
    {
        // snap back to the parent
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;

        EventSystem.current.RaycastAll(eventData, hits);
        foreach (RaycastResult hit in hits)
        {
            // is there a slot underneath us?
            InventorySlot slotFound = hit.gameObject.GetComponent<InventorySlot>();
            if (slotFound != null && slotFound != slot)
            {
                // If the item was dropped in the same inventory as it is currently 
                // in, then the player is reordering the items in the inventory. 
                // In this case, we should not trigger the OnItemTransaction event.
                if (slot.parentInventory != slotFound.parentInventory)
                {
                    // inventories are not the same - we are moving between 
                    // inventories
                    if (slotFound.draggableItem != null)
                    {
                        // we need to keep a copy of the items we are swapping, 
                        // because once we add and remove items from the inventory, 
                        // the item values will change
                        InventoryItem ours = item;
                        InventoryItem theirs = slotFound.draggableItem.item;

                        slot.parentInventory.inventory.RemoveItem(ours, slot.index);
                        slot.draggableItem.SetItem(theirs);

                        slotFound.parentInventory.inventory.AddItem(ours, slotFound.index);
                        slotFound.draggableItem.SetItem(ours);
                    }
                }
                return;
            }
        }


    }

    protected void Swap(InventorySlot newParent)
    {
        DraggableItem other = newParent.draggableItem as DraggableItem;
        if (other)
        {
            InventoryItem ours = item;
            InventoryItem theirs = other.item;
            
            slot.UpdateItem(theirs);
            other.slot.UpdateItem(ours);
        }
    }
}

