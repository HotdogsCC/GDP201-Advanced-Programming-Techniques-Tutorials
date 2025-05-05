using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.InputSystem;


public class InventoryPlayTests
{
    private InventoryUI ArrangeUI()
    {
        
        // Create a UI canvas
        GameObject canvasGameObject = new GameObject("Canvas");
        Canvas canvas = canvasGameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        
        canvasGameObject.AddComponent<GraphicRaycaster>();
        
        // Add event system for drag events
        if (EventSystem.current == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
        
        // create the info pop up box (just with the script so it exists)
        GameObject infoBox = new GameObject("Info Box");
        infoBox.AddComponent<InfoBox>();
        infoBox.transform.position = new Vector3(-9999, -9999, -9999);
        GameObject nameGO = new GameObject("Name TMPro");
        var nameTMPro = nameGO.AddComponent<TextMeshProUGUI>();
        GameObject descGO = new GameObject("Description TMPro");
        var descTMPro = descGO.AddComponent<TextMeshProUGUI>();
        GameObject priceGO = new GameObject("Price TMPro");
        var priceTMPro = priceGO.AddComponent<TextMeshProUGUI>();
        infoBox.GetComponent<InfoBox>().SetUpTMPro(nameTMPro, descTMPro, priceTMPro);
        
        // create the money ui
        GameObject moneyUI = new GameObject("Money");
        TextMeshProUGUI moneyTMpro = moneyUI.AddComponent<TextMeshProUGUI>();
        
        
        // create the player wallet with lots of money
        GameObject playerWallet = new GameObject("Player Wallet");
        Wallet walletComponent = playerWallet.AddComponent<Wallet>();
        walletComponent.uiWidget = moneyTMpro;
        walletComponent.Money = 9999;
        
        
        
        // Create the inventory UI game object with InventoryUI component
        GameObject inventoryUIGameObject = new GameObject("InventoryUI");
        //Make it a child of the Canvas
        inventoryUIGameObject.transform.SetParent(canvas.transform);
        GridLayoutGroup gridLayoutGroup = inventoryUIGameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(128, 128);
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
        inventoryUI.inventory.items = new InventoryItem[8];
        inventoryUI.inventory.items[0] = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryUI.inventory.items[0].name = "Sword";
        inventoryUI.inventory.items[0].itemName = "Sword";
        inventoryUI.inventory.items[0].price = 1;
        
        inventoryUI.inventory.items[1] = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryUI.inventory.items[1].name = "Shield";
        inventoryUI.inventory.items[1].itemName = "Shield";
        inventoryUI.inventory.items[1].price = 1;

        // Load the slot and item prefabs needed to initialise the InventoryUI
        inventoryUI.slotPrefab = Resources.Load<InventorySlot>("Prefabs/InventorySlot");
        inventoryUI.itemPrefab = Resources.Load<DraggableItem>("Prefabs/InventoryItem");
        
        // Call initialise() to setup the inventory ui
        inventoryUI.Init();

        return inventoryUI;
    }
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DragItemToNewSlotAndSwapItems()
    {
        // Arrange: Set up two InventorySlot instances and assign items
        // Act: Simulate dragging a DraggableItem from one slot to another
        // Assert: Verify the items swapped
        
        //Arrange
        InventoryUI inventoryUI = ArrangeUI();
        
        //Act

        yield return null;

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

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        //Act
        
        yield return null;
        
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
        RectTransform targetSlotRect = inventory2UI.slots[1].GetComponent<RectTransform>();
        
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
        
        Assert.AreEqual("Sword", inventory2UI.inventory.items[1].name);
        Assert.AreEqual("Sword", inventory2UI.slots[1].draggableItem.item.name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator WalletUIDisplaysCorrectAmount()
    {
        // Arrange: Create a canvas with a tmpro game object,
        // create a wallet game object, and assign the tmpro to wallet
        
        //Act: Run a frame, the code should run on start to displat amount
        
        //Assert: confirm the amount being displayed is expected
        
        //arrange
        
        //create canvas
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
        
        // create the money ui
        GameObject moneyUI = new GameObject("Money");
        TextMeshProUGUI moneyTMpro = moneyUI.AddComponent<TextMeshProUGUI>();
        
        
        // create the player wallet with lots of money
        GameObject playerWallet = new GameObject("Player Wallet");
        Wallet walletComponent = playerWallet.AddComponent<Wallet>();
        walletComponent.uiWidget = moneyTMpro;
        walletComponent.Money = 9999;
        
        //act
        yield return null;
        
        //assert
        Assert.AreEqual("$9999", moneyTMpro.text);
    }

    [UnityTest]
    public IEnumerator DragItemToSameInventoryEmptySlot()
    {
        //Arrange
        InventoryUI inventoryUI = ArrangeUI();
        
        //act

        yield return null;
        
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
        // Simulate dragging over to the target slot (Slot 2, which is nothing)
        RectTransform targetSlotRect = inventoryUI.slots[2].GetComponent<RectTransform>();
        
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
        Assert.IsNull(inventoryUI.inventory.items[0]);
        
        Assert.AreEqual("Sword", inventoryUI.inventory.items[2].name);
        Assert.AreEqual("Sword", inventoryUI.slots[2].draggableItem.item.name);

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator DragItemToNewInventoryEmptySlot()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        //Act
        
        yield return null;
        
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
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
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
        Assert.IsNull(inventory1UI.inventory.items[0]);
        
        Assert.AreEqual("Sword", inventory2UI.inventory.items[2].name);
        Assert.AreEqual("Sword", inventory2UI.slots[2].draggableItem.item.name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SavesAndLoadsOneInventory()
    {
        // this test does NOT test the act of pressing the on screen save and load icons,
        // and only tests the functionally pressing what the icons would do.
        
        // arrange: need a user interface initialised with some assets
        // act: save the ui, alter it, load the save
        // assert: and check it has been restored correctly
        
        // arrange: 
        InventoryUI inventoryUI = ArrangeUI();
        
        //act:
        yield return null;
        
        //save the inventory
        inventoryUI.inventory.SaveInventory("TEXT_inventory.json");

        //alter the inventory
        inventoryUI.inventory.items[0].itemName = "SomethingItShouldNotBe"; 
        inventoryUI.inventory.items[1].itemName = "SomethingItShouldNotBeEither";
        
        //load the inventory
        inventoryUI.inventory.LoadInventory("TEXT_inventory.json");

        yield return null;
        //assert
        
        //check the inventory data 
        Assert.AreEqual("Sword", inventoryUI.inventory.items[0].itemName);
        Assert.AreEqual("Shield", inventoryUI.inventory.items[1].itemName);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator SavesAndLoadsWallet()
    {
        // this test does NOT test the act of pressing the on screen
        // save and load icons, and only tests the functionally pressing
        // the icons would do

        // arrange: need a wallet to store cash and a TextMeshPro asset to display it
        
        // act: save the wallet, run a frame,  alter it, then revert to the save
        
        // assert: confirm the loading back of the wallet resulted in the original wallet from the save
        
        // arrange
        
        //create canvas
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
        
        // create the money ui
        GameObject moneyUI = new GameObject("Money");
        TextMeshProUGUI moneyTMpro = moneyUI.AddComponent<TextMeshProUGUI>();
        
        
        // create the player wallet with $100
        GameObject playerWallet = new GameObject("Player Wallet");
        Wallet walletComponent = playerWallet.AddComponent<Wallet>();
        walletComponent.uiWidget = moneyTMpro;
        walletComponent.Money = 100;
        
        //act 
        yield return null;
        
        //save the wallet
        walletComponent.SaveWallet("TEST_wallet.txt");
        
        //change the value of the wallet
        walletComponent.Money = 321;
        
        //reload previous wallet (which should be $100)
        walletComponent.LoadWallet("TEST_wallet.txt");
        
        //assert
        
        //confirm the value of wallet is 100
        Assert.AreEqual(100, walletComponent.Money);
        //confirm the UI display of wallet is "$100"
        Assert.AreEqual("$100", moneyTMpro.text);

    }

    [UnityTest]
    public IEnumerator InfoBoxPopUpDisplaysOnHover()
    {
        //arrange: set up inventory UI with an item with information
        //act: place mouse cursor over item
        //assert: confirm the information box has moved position
        
        //arrange
        InventoryUI inventoryUI = ArrangeUI();

        yield return null;
        
        //act 
        
        //Simulate hovering on slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the draggable item
        RectTransform draggableItemRect = inventoryUI.slots[0].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start hovering
        inventoryUI.slots[0].draggableItem.OnPointerEnter(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        
        //assert
        InfoBox infoBox = GameObject.FindObjectOfType<InfoBox>();
        //confirms its transform has moved from its starting place
        Assert.IsTrue(infoBox.transform.position != new Vector3(-9999, -9999, -9999));
        
    }

    [UnityTest]
    public IEnumerator InfoBoxPopUpDisappearsOnMouseExit()
    {
        //arrange: set up inventory UI with an item with information
        //act: place mouse cursor over item, then remove cursor
        //assert: confirm the information box is gone
        
        //arrange
        InventoryUI inventoryUI = ArrangeUI();

        yield return null;
        
        //act 
        
        //Simulate hovering on slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the draggable item
        RectTransform draggableItemRect = inventoryUI.slots[0].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start hovering
        inventoryUI.slots[0].draggableItem.OnPointerEnter(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        
        // stop hovering
        inventoryUI.slots[0].draggableItem.OnPointerExit(pointerEventData);
        yield return null;
        
        //assert
        InfoBox infoBox = GameObject.FindObjectOfType<InfoBox>();
        //confirms its transform has moved from its starting place
        Assert.IsTrue(infoBox.transform.position == new Vector3(999999, 999999, 999999));
    }
    
    [UnityTest]
    public IEnumerator InfoBoxPopUpDisappearsOnDrag()
    {
        //arrange: set up inventory UI with an item with information
        //act: place mouse cursor over item, then drag cursor
        //assert: confirm the information box is gone
        
        //arrange
        InventoryUI inventoryUI = ArrangeUI();

        yield return null;
        
        //act 
        
        //Simulate hovering on slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the draggable item
        RectTransform draggableItemRect = inventoryUI.slots[0].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start hovering
        inventoryUI.slots[0].draggableItem.OnPointerEnter(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        
        // start dragging
        inventoryUI.slots[0].draggableItem.OnBeginDrag(pointerEventData);
        yield return null;
        
        //assert
        InfoBox infoBox = GameObject.FindObjectOfType<InfoBox>();
        //confirms its transform has moved from its starting place
        Assert.IsTrue(infoBox.transform.position == new Vector3(999999, 999999, 999999));
    }
    
    [UnityTest]
    public IEnumerator BuyItemOnDragToNewInventoryEmptySlot()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        // set names
        inventory1UI.name = "Chest Inventory";
        inventory2UI.name = "Player Inventory";
        
        //Act
        
        yield return null;
        
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
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[0].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[0].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if balance decreased
        Wallet wallet = GameObject.FindObjectOfType<Wallet>();
        Assert.AreEqual(9998, wallet.Money);

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator SellItemOnDragToNewInventoryEmptySlot()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        // set names
        inventory1UI.name = "Player Inventory";
        inventory2UI.name = "Chest Inventory";
        
        //Act
        
        yield return null;
        
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
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[0].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[0].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if balance decreased
        Wallet wallet = GameObject.FindObjectOfType<Wallet>();
        Assert.AreEqual(10000, wallet.Money);

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator FailToBuyItemOnDragToNewInventoryEmptySlotWhenBroke()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items did not move because the player cannot afford it
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        // set names
        inventory1UI.name = "Chest Inventory";
        inventory2UI.name = "Player Inventory";

        GameObject.FindObjectOfType<Wallet>().Money = 0;
        
        //Act
        
        yield return null;
        
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
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[0].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[0].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if items did not move
        Assert.AreEqual("Sword", inventory1UI.slots[0].draggableItem.item.itemName);

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator SwordMeshDisplaysOnPurchase()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item, and the item equipper
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        // set names
        inventory1UI.name = "Chest Inventory";
        inventory2UI.name = "Player Inventory";
        
        //set up equipper
        InventoryEquip equipper = inventory2UI.inventory.gameObject.AddComponent<InventoryEquip>();
        equipper.playerInventory = inventory2UI.inventory;

        GameObject swordObject = new GameObject("Sword");
        GameObject shieldObject = new GameObject("Shield");
        swordObject.SetActive(false);
        shieldObject.SetActive(false);
        equipper.swordGameObject = swordObject;
        equipper.shieldGameObject = shieldObject;
        
        //Act
        
        yield return null;
        
        //Simulate dragging from slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the sword item
        RectTransform draggableItemRect = inventory1UI.slots[0].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start dragging
        inventory1UI.slots[0].draggableItem.OnBeginDrag(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[0].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[0].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if sword geometry is active
        Assert.IsTrue(swordObject.activeSelf);

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator ShieldMeshDisplaysOnPurchase()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item, and the item equipper
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        // set names
        inventory1UI.name = "Chest Inventory";
        inventory2UI.name = "Player Inventory";
        
        //set up equipper
        InventoryEquip equipper = inventory2UI.inventory.gameObject.AddComponent<InventoryEquip>();
        equipper.playerInventory = inventory2UI.inventory;

        GameObject swordObject = new GameObject("Sword");
        GameObject shieldObject = new GameObject("Shield");
        swordObject.SetActive(false);
        shieldObject.SetActive(false);
        equipper.swordGameObject = swordObject;
        equipper.shieldGameObject = shieldObject;
        
        //Act
        
        yield return null;
        
        //Simulate dragging from slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the shield item
        RectTransform draggableItemRect = inventory1UI.slots[1].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start dragging
        inventory1UI.slots[0].draggableItem.OnBeginDrag(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[1].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[1].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if sword geometry is active
        Assert.IsTrue(shieldObject.activeSelf);

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator SwordMeshHidesOnSell()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item, and the item equipper
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        // set names
        inventory1UI.name = "Player Inventory";
        inventory2UI.name = "Chest Inventory";
        
        //set up equipper
        InventoryEquip equipper = inventory1UI.inventory.gameObject.AddComponent<InventoryEquip>();
        equipper.playerInventory = inventory1UI.inventory;

        GameObject swordObject = new GameObject("Sword");
        GameObject shieldObject = new GameObject("Shield");
        swordObject.SetActive(true);
        shieldObject.SetActive(true);
        equipper.swordGameObject = swordObject;
        equipper.shieldGameObject = shieldObject;
        
        //Act
        
        yield return null;
        
        //Simulate dragging from slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the sword item
        RectTransform draggableItemRect = inventory1UI.slots[0].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start dragging
        inventory1UI.slots[0].draggableItem.OnBeginDrag(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[0].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[0].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if sword geometry is not active
        Assert.IsTrue(!swordObject.activeSelf);

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator ShieldMeshHidesOnSell()
    {
        // Arrange: Set up 2 Inventories, both with their own UI and 1 item, and the item equipper
        // Act: Simulate dragging a DraggableItem from one inventory to another
        // Assert: Verify the items swapped
        
        //Arrange

        InventoryUI inventory1UI = ArrangeUI();
        InventoryUI inventory2UI = ArrangeUI();
        
        // set positions
        inventory1UI.transform.position = new Vector3(-288, 0, 0);
        inventory2UI.transform.position = new Vector3(288, 0, 0);
        
        // set names
        inventory1UI.name = "Player Inventory";
        inventory2UI.name = "Chest Inventory";
        
        //set up equipper
        InventoryEquip equipper = inventory1UI.inventory.gameObject.AddComponent<InventoryEquip>();
        equipper.playerInventory = inventory1UI.inventory;

        GameObject swordObject = new GameObject("Sword");
        GameObject shieldObject = new GameObject("Shield");
        swordObject.SetActive(true);
        shieldObject.SetActive(true);
        equipper.swordGameObject = swordObject;
        equipper.shieldGameObject = shieldObject;
        
        //Act
        
        yield return null;
        
        //Simulate dragging from slot 0
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        //Set initial pointer position to the centre of the shield item
        RectTransform draggableItemRect = inventory1UI.slots[1].draggableItem.GetComponent<RectTransform>();
        Vector3 startPosition = draggableItemRect.position;
        pointerEventData.position = startPosition;
        
        // Start dragging
        inventory1UI.slots[1].draggableItem.OnBeginDrag(pointerEventData);
        //We need to yield to allow the Event system to update
        yield return null;
        // Simulate dragging over to the target slot (in the 2nd inventory) (and it has nothing)
        RectTransform targetSlotRect = inventory2UI.slots[2].GetComponent<RectTransform>();
        
        //Hover mouse over target position
        pointerEventData.position = targetSlotRect.position;
        
        inventory1UI.slots[1].draggableItem.OnDrag(pointerEventData);
        
        //We need to yield to allow the Event system to update
        yield return null;
        
        // End dragging
        inventory1UI.slots[1].draggableItem.OnEndDrag(pointerEventData);
        
        yield return null;
        //Assert
        
        //Check if sword geometry is not active
        Assert.IsTrue(!shieldObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CoinCollisionIncreasesWalletMoney()
    {
        //arrange: wallet and coin
        //act: mimic a collision in the coin
        //assert: verify wallet has risen
        
        //arrange
        
        // create the money ui
        GameObject moneyUI = new GameObject("Money");
        TextMeshProUGUI moneyTMpro = moneyUI.AddComponent<TextMeshProUGUI>();
        
        // create the player wallet
        GameObject playerWallet = new GameObject("Player Wallet");
        Wallet walletComponent = playerWallet.AddComponent<Wallet>();
        walletComponent.uiWidget = moneyTMpro;
        walletComponent.Money = 100;
        
        // create the coin
        Coin coin = new GameObject().AddComponent<Coin>();
        
        yield return null;
        
        //act
        
        
        coin.MimicCollisionForTest();
        
        //assert
        
        //check money rose by 100
        Assert.AreEqual(walletComponent.Money, 200);
    }

    [UnityTest]
    public IEnumerator ChestOpenUnlocksCursor()
    {
        //arrange: set up a chest
        //act: open the chest
        //assert: confirm the cursor is unlocked

        //arrange
        
        Chest chest = new GameObject().AddComponent<Chest>();
        chest.inputActionAsset = Resources.Load<InputActionAsset>("InputSystem_Actions");

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<PlayerInput>().actions = chest.inputActionAsset;

        InventoryUI inv1 = ArrangeUI();
        InventoryUI inv2 = ArrangeUI();
        chest.playerInventory = inv1;
        chest.chestInventory = inv2;
        
        yield return null;
        
        //act
        
        chest.OnOpen();
        
        Assert.IsTrue(Cursor.lockState == CursorLockMode.None);
    }
    
    [UnityTest]
    public IEnumerator ChestCloseLocksCursor()
    {
        //arrange: set up a chest
        //act: open the chest
        //assert: confirm the cursor is unlocked

        //arrange
        
        Chest chest = new GameObject().AddComponent<Chest>();
        chest.inputActionAsset = Resources.Load<InputActionAsset>("InputSystem_Actions");

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<PlayerInput>().actions = chest.inputActionAsset;

        InventoryUI inv1 = ArrangeUI();
        InventoryUI inv2 = ArrangeUI();
        chest.playerInventory = inv1;
        chest.chestInventory = inv2;
        
        yield return null;
        
        //act
        
        chest.OnClose();
        
        Assert.IsTrue(Cursor.lockState == CursorLockMode.Locked);
    }
}
