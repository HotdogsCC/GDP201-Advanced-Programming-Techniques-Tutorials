using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    // A Test behaves as an ordinary method
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

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator InventoryTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
