using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Saver : MonoBehaviour
{
    [Header("Save Locations")]
    [SerializeField] private string playerInventorySaveLocation;
    [SerializeField] private string chestInventorySaveLocation;
    [SerializeField] private string walletSaveLocation;

    [Header("Object References")]
    [SerializeField] private InventoryUI playerInventory;
    [SerializeField] private InventoryUI chestInventory;
    [SerializeField] private Wallet wallet;
    [SerializeField] private InventoryEquip inventoryEquip;

    private void Start()
    {
        //load save on boot
        //LoadAll();
    }

    public void SaveAll()
    {
        playerInventory.Save(playerInventorySaveLocation);
        chestInventory.Save(chestInventorySaveLocation);
        wallet.SaveWallet(walletSaveLocation);
    }

    public void LoadAll()
    {
        playerInventory.Load(playerInventorySaveLocation);
        chestInventory.Load(chestInventorySaveLocation);
        wallet.LoadWallet(walletSaveLocation);
        inventoryEquip.LoadEquip();
    }
}
