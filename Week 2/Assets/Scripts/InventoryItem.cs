using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem", order = 1)]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    [Multiline(3)]
    public string description;

    public Sprite icon;
    public Color color = Color.white;

    public int price;
}

[System.Serializable]
public class SerializableInventoryItem
{
    public string itemName;
    public string description;
    public string iconPath; // Path to load sprite icons
    public Color color;

    public SerializableInventoryItem(InventoryItem item)
    {
        itemName = item.itemName;
        description = item.description;
        color = item.color;

        // Save the sprite icon path for later use
        iconPath =
           item.icon != null ? string.Format("Icons/{0}", item.icon.name) : null;
    }

    public InventoryItem ToInventoryItem()
    {
        InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
        newItem.itemName = itemName;
        newItem.description = description;
        newItem.color = color;

        if (!string.IsNullOrEmpty(iconPath))
        {
            newItem.icon = Resources.Load<Sprite>(iconPath);
        }

        return newItem;
    }
}

[System.Serializable]
public class SerializableInventory
{
    public SerializableInventoryItem[] items;

    public SerializableInventory(Inventory inventory)
    {
        items = new SerializableInventoryItem[inventory.items.Length];
        for (int i = 0; i < inventory.items.Length; i++)
        {
            if (inventory.items[i] != null)
                items[i] = new SerializableInventoryItem(inventory.items[i]);
        }
    }

    public void ApplyToInventory(Inventory inventory)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                inventory.items[i] = items[i].ToInventoryItem();
        }
    }
}
