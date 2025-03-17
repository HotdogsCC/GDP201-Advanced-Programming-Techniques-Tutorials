using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryItem[] items;

    public DraggableItem itemPrefab;
    public InventorySlot slotPrefab;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        //create a slot
        slots = new InventorySlot[items.Length];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
