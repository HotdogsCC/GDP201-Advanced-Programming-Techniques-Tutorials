using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public bool dragging;
    public Transform originalParent;
    public Canvas canvas;

    public InventorySlot slot;

    public Image image;

    public InventoryItem item;

    private InfoBox infoBox;

    private void Start()
    {
        infoBox = FindObjectOfType<InfoBox>();
        if (infoBox == null)
        {
            Debug.Log("Info box was failed to be found");
        }
    }

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

            if (item != null)
            {
                if (item.price != 0)
                {
                    gameObject.SetActive(true);
                    return;
                }
            }
            gameObject.SetActive(false);
            
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
        
        infoBox.Vanish();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragging)
        {
            transform.position = eventData.position;
            infoBox.Vanish();
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
                Debug.Log("there is a slot under neath me");
                // If the item was dropped in the same inventory as it is currently 
                // in, then the player is reordering the items in the inventory. 
                // In this case, we should not trigger the OnItemTransaction event.
                if (slot.parentInventory != slotFound.parentInventory)
                {
                    Debug.Log("i am going into a different inventory");
                    // inventories are not the same - we are moving between inventories
                    if (slotFound.draggableItem != null)
                    {
                        Debug.Log("the item is not null");
                        // we need to keep a copy of the items we are swapping, 
                        // because once we add and remove items from the inventory, 
                        // the item values will change
                        InventoryItem ours = item;
                        InventoryItem theirs = slotFound.draggableItem.item;

                        Wallet playerWallet = FindObjectOfType<Wallet>();
                        if (playerWallet == null)
                        {
                            GameLogger.LogWarning("player wallet not found");
                            return;
                        }

                        //checks if the item is being moved to the player inventory
                        //i.e. checking if an item is being sold
                        if (slotFound.parentInventory.gameObject.name == "Player Inventory")
                        {
                            Debug.Log("this is the player inventory");
                            //checks the player can afford the purchase
                            if (playerWallet.Money < ours.price)
                            {
                                Debug.Log("not enough money");
                                //doesnt run the swap if the player cannot aford it
                                return;
                            }
                        }
                        
                        Debug.Log("swappin");
                        slot.parentInventory.inventory.AddItem(theirs, slot.index);
                        slot.draggableItem.SetItem(theirs);

                        slotFound.parentInventory.inventory.AddItem(ours, slotFound.index);
                        slotFound.draggableItem.SetItem(ours);
                    }
                }
                else
                {
                    Debug.Log("i am going into the same inventory");
                    //swaps items in same inventory
                    Swap(slotFound);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoBox.Summon(transform.position, item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.Vanish();
    }
}

