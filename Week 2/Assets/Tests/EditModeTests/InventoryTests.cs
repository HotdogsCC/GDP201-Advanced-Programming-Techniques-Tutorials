using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    [Test]
    public void InitialiseInventorySlotWithItem()
    {
        //Arrange: Set up an InventoryItem and an InventorySlot
        //Act: Initialise the InventorySlot with the InventoryItem
        //Assert: Verify that the InventorySlot contains the expected item
        
        //Arrange
        InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryItem.name = "Sword";

        InventorySlot inventorySlot = new GameObject().AddComponent<InventorySlot>();
        DraggableItem draggableItem = new GameObject().AddComponent<DraggableItem>();
        
        //Act
        draggableItem.SetItem(inventoryItem);
        inventorySlot.Init(null, 0, draggableItem);
        
        //Assert
        Assert.AreEqual("Sword", inventorySlot.draggableItem.item.name);
    }
    
    [Test]
    public void InitialiseInventorySlotWithoutItem()
    {
        //Arrange: Set up an InventorySlot
        //Act: Verify if the draggableItem (which holds the item) is null right after initialization
        //Assert: Verify that the InventorySlot contains the nothing
        
        //Arrange
        InventorySlot inventorySlot = new GameObject().AddComponent<InventorySlot>();
        
        //Act
        bool isNull = inventorySlot.draggableItem == null;
        
        //Assert
        Assert.IsTrue(isNull);
    }
    
}
