using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryEquip : MonoBehaviour
{
    [Header("Inventory Reference")]
    [SerializeField] public Inventory playerInventory;
    
    [Header("Equip Object References")]
    [SerializeField] public GameObject swordGameObject;
    [SerializeField] public GameObject shieldGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        swordGameObject.SetActive(false);
        shieldGameObject.SetActive(false);
        playerInventory.OnItemTransaction += TryToEquip;
    }

    void TryToEquip(InventoryItem item, string transactionType)
    {
        switch (transactionType)
        {
            case "buy":
                if (item.itemName.Contains("Sword"))
                {
                    swordGameObject.SetActive(true);
                }

                else if (item.itemName.Contains("Shield"))
                {
                    shieldGameObject.SetActive(true);
                }
                break;
            
            case "sell":
                if (item.itemName.Contains("Sword"))
                {
                    swordGameObject.SetActive(false);
                }

                else if (item.itemName.Contains("Shield"))
                {
                    shieldGameObject.SetActive(false);
                }
                break;
        }
        
        
    }

    public void LoadEquip()
    {
        swordGameObject.SetActive(false);
        shieldGameObject.SetActive(false);
        for (int i = 0; i < playerInventory.items.Length; i++)
        {
            if (playerInventory.items[i].itemName.Contains("Sword"))
            {
                swordGameObject.SetActive(true);
            }

            else if (playerInventory.items[i].itemName.Contains("Shield"))
            {
                shieldGameObject.SetActive(true);
            }
        }
    }
}
