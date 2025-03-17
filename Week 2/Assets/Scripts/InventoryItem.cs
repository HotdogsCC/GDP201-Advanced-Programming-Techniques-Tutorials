using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem", order = 1)]
public class InventoryItem : ScriptableObject
{
    string itemName;
    [Multiline(3)]
    string description;

    public Sprite icon;
    public Color color = Color.white;
}
