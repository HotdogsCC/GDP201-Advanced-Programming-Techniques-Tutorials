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
        //snap back to the parent
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        
        //is there a slot underneath us?
        InventorySlot slotFound = null;
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (RaycastResult hit in hits)
        {
            InventorySlot s = hit.gameObject.GetComponent<InventorySlot>();
            if (s)
            {
                slotFound = s;
            }
        }

        if (slotFound)
        {
            //swap the items around for the slot we found and ourselves
            Swap(slotFound);
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

