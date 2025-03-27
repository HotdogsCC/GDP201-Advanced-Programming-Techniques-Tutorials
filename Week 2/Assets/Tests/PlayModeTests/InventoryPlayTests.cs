using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;


public class InventoryPlayTests
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DragItemToNewSlotAndSwapItems()
    {
        // Arrange: Set up two InventorySlot instances and assign items
        // Act: Simulate dragging a DraggableItem from one slot to another
        // Assert: Verify the items swapped
        
        //Arrange
        
        // Create a UI canvas
        GameObject canvasGameObject = new GameObject("Canvas");
        Canvas canvas = canvasGameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGameObject.AddComponent<CanvasScaler>();
        canvasGameObject.AddComponent<GraphicRaycaster>();
        
        // Add event system for drag events
        if (EventSystem.current == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
        
        // Create the inventory game object with InventoryUI component
        GameObject inventoryGameObject = new GameObject("Inventory");
        //Make it a child of the Canvas
        inventoryGameObject.transform.SetParent(canvas.transform);
        GridLayoutGroup gridLayoutGroup = inventoryGameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(64, 64);
        gridLayoutGroup.spacing = new Vector2(2, 2);
        gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 4;
        InventoryUI inventoryUI = inventoryGameObject.AddComponent<InventoryUI>();
        
        //Init the items array for InventoryUI
        inventoryUI.inventory.items = new InventoryItem[2];
        inventoryUI.inventory.items[0] = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryUI.inventory.items[0].name = "Sword";
        
        inventoryUI.inventory.items[1] = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryUI.inventory.items[1].name = "Shield";
        
        // Load the slot and item prefabs needed to initialise the InventoryUI
        inventoryUI.slotPrefab = Resources.Load<InventorySlot>("Prefabs/InventorySlot");
        inventoryUI.itemPrefab = Resources.Load<DraggableItem>("Prefabs/InventoryItem");
        
        // Call initialise() to setup the inventory ui
        inventoryUI.Init();
        
        //Act
        
        //Simulate dragging from slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the draggble item
        RectTransform draggableItemRect = inventoryUI.slots[0].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start dragging
        inventoryUI.slots[0].draggableItem.OnBeginDrag(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        // Simulate dragging over to the target slot (Slot 1)
        RectTransform targetSlotRect = inventoryUI.slots[1].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventoryUI.slots[0].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventoryUI.slots[0].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if items are swapped
        Assert.AreEqual("Shield", inventoryUI.inventory.items[0].name);
        Assert.AreEqual("Shield", inventoryUI.slots[0].draggableItem.item.name);
        
        Assert.AreEqual("Sword", inventoryUI.inventory.items[1].name);
        Assert.AreEqual("Sword", inventoryUI.slots[1].draggableItem.item.name);

        yield return null;
    }
}
