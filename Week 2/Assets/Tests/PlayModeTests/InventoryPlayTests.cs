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
        
        // Create the inventory UI game object with InventoryUI component
        GameObject inventoryUIGameObject = new GameObject("InventoryUI");
        //Make it a child of the Canvas
        inventoryUIGameObject.transform.SetParent(canvas.transform);
        GridLayoutGroup gridLayoutGroup = inventoryUIGameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(64, 64);
        gridLayoutGroup.spacing = new Vector2(2, 2);
        gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 4;
        InventoryUI inventoryUI = inventoryUIGameObject.AddComponent<InventoryUI>();
        
        // Create the inventory game object (one that holds the data)
        GameObject inventoryGameObject = new GameObject("Inventory");
        Inventory inventoryComponent = inventoryGameObject.AddComponent<Inventory>();
        
        //Assign the inventory to the inventory UI
        inventoryUI.inventory = inventoryComponent;
        
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

    [UnityTest]
    public IEnumerator DragItemToNewInventoryAndSwapItems()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange
        
        // Create the UI canvas
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
        
        // Create the inventory UI game object with InventoryUI component
        GameObject inventory1UIGameObject = new GameObject("InventoryUI");
        //Make it a child of the Canvas
        inventory1UIGameObject.transform.SetParent(canvas.transform);
        GridLayoutGroup gridLayoutGroup1 = inventory1UIGameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup1.cellSize = new Vector2(64, 64);
        gridLayoutGroup1.spacing = new Vector2(2, 2);
        gridLayoutGroup1.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayoutGroup1.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayoutGroup1.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup1.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup1.constraintCount = 4;
        InventoryUI inventory1UI = inventory1UIGameObject.AddComponent<InventoryUI>();
        
        // Create the inventory game object (one that holds the data)
        GameObject inventory1GameObject = new GameObject("Inventory");
        Inventory inventory1Component = inventory1GameObject.AddComponent<Inventory>();
        
        //Assign the inventory to the inventory UI
        inventory1UI.inventory = inventory1Component;
        
        //Init the items array for InventoryUI
        inventory1UI.inventory.items = new InventoryItem[1];
        inventory1UI.inventory.items[0] = ScriptableObject.CreateInstance<InventoryItem>();
        inventory1UI.inventory.items[0].name = "Sword";
        
        // Load the slot and item prefabs needed to initialise the InventoryUI
        inventory1UI.slotPrefab = Resources.Load<InventorySlot>("Prefabs/InventorySlot");
        inventory1UI.itemPrefab = Resources.Load<DraggableItem>("Prefabs/InventoryItem");
        
        // Create the inventory UI game object with InventoryUI component
        GameObject inventory2UIGameObject = new GameObject("InventoryUI2");
        //Make it a child of the Canvas
        inventory2UIGameObject.transform.SetParent(canvas.transform);
        GridLayoutGroup gridLayoutGroup2 = inventory2UIGameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup2.cellSize = new Vector2(64, 64);
        gridLayoutGroup2.spacing = new Vector2(2, 2);
        gridLayoutGroup2.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayoutGroup2.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayoutGroup2.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup2.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup2.constraintCount = 4;
        InventoryUI inventory2UI = inventory2UIGameObject.AddComponent<InventoryUI>();
        
        // Create the inventory game object (one that holds the data)
        GameObject inventory2GameObject = new GameObject("Inventory2");
        Inventory inventory2Component = inventory2GameObject.AddComponent<Inventory>();
        
        //Assign the inventory to the inventory UI
        inventory2UI.inventory = inventory2Component;
        
        //Init the items array for InventoryUI
        inventory2UI.inventory.items = new InventoryItem[1];
        
        inventory2UI.inventory.items[0] = ScriptableObject.CreateInstance<InventoryItem>();
        inventory2UI.inventory.items[0].name = "Shield";
        
        // Load the slot and item prefabs needed to initialise the InventoryUI
        inventory2UI.slotPrefab = Resources.Load<InventorySlot>("Prefabs/InventorySlot");
        inventory2UI.itemPrefab = Resources.Load<DraggableItem>("Prefabs/InventoryItem");
        
        // Call initialise() to setup the inventory ui
        inventory1UI.Init();
        inventory2UI.Init();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        //Act
        
        //Simulate dragging from slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the draggable item
        RectTransform draggableItemRect = inventory1UI.slots[0].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start dragging
        inventory1UI.slots[0].draggableItem.OnBeginDrag(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        // Simulate dragging over to the target slot (in the 2nd inventory)
        RectTransform targetSlotRect = inventory2UI.slots[0].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[0].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[0].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if items are swapped
        Assert.AreEqual("Shield", inventory1UI.inventory.items[0].name);
        Assert.AreEqual("Shield", inventory1UI.slots[0].draggableItem.item.name);
        
        Assert.AreEqual("Sword", inventory2UI.inventory.items[0].name);
        Assert.AreEqual("Sword", inventory2UI.slots[0].draggableItem.item.name);

        yield return null;
    }
}
