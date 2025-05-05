using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
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

        InventorySlot inventorySlot = new GameObject().AddComponent<InventorySlot>();
        DraggableItem draggableItem = new GameObject().AddComponent<DraggableItem>();
        
        //Act
        draggableItem.SetItem(inventoryItem);
        inventorySlot.Init(null, 0, draggableItem);
        
        //Assert
        Assert.IsTrue(inventorySlot.draggableItem);
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

    [Test]
    public void InitialiseInfoBoxWithItemInformation()
    {
        //Arrange: Set up an InfoBox and an Item
        //Act: Set the InfoBox params with the Item
        //Assert: Verify text is matching to item
        
        //Arrange
        InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryItem.name = "Sword";
        inventoryItem.itemName = "Sword";
        inventoryItem.description = "Well used";
        inventoryItem.price = 2;

        InfoBox infoBox = new GameObject("Info Box").AddComponent<InfoBox>();

        TextMeshProUGUI nameTMP = new GameObject().AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI descTMP = new GameObject().AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceTMP = new GameObject().AddComponent<TextMeshProUGUI>();
        
        infoBox.SetUpTMPro(nameTMP, descTMP, priceTMP);
        
        //Act
        infoBox.SetContents(inventoryItem);
        
        //Assert
        Assert.AreEqual("Sword", nameTMP.text);
        Assert.AreEqual("Well used", descTMP.text);
        Assert.AreEqual("$ 2", priceTMP.text);
    }
    
    [Test]
    public void InitialiseInventorySlotWithName()
    {
        //Arrange: Set up an InventoryItem and an InventorySlot
        //Act: Initialise the InventorySlot with the InventoryItem and a name
        //Assert: Verify that the InventorySlot contains the expected name
        
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
    public void InitialiseInventorySlotWithDescription()
    {
        //Arrange: Set up an InventoryItem and an InventorySlot
        //Act: Initialise the InventorySlot with the InventoryItem and a name
        //Assert: Verify that the InventorySlot contains the expected name
        
        //Arrange
        InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryItem.description = "Standard information";

        InventorySlot inventorySlot = new GameObject().AddComponent<InventorySlot>();
        DraggableItem draggableItem = new GameObject().AddComponent<DraggableItem>();
        
        //Act
        draggableItem.SetItem(inventoryItem);
        inventorySlot.Init(null, 0, draggableItem);
        
        //Assert
        Assert.AreEqual("Standard information", inventorySlot.draggableItem.item.description);
    }
    
    [Test]
    public void InitialiseInventorySlotWithPrice()
    {
        //Arrange: Set up an InventoryItem and an InventorySlot
        //Act: Initialise the InventorySlot with the InventoryItem and a name
        //Assert: Verify that the InventorySlot contains the expected name
        
        //Arrange
        InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryItem.price = 12345;

        InventorySlot inventorySlot = new GameObject().AddComponent<InventorySlot>();
        DraggableItem draggableItem = new GameObject().AddComponent<DraggableItem>();
        
        //Act
        draggableItem.SetItem(inventoryItem);
        inventorySlot.Init(null, 0, draggableItem);
        
        //Assert
        Assert.AreEqual(12345, inventorySlot.draggableItem.item.price);
    }
    
}
