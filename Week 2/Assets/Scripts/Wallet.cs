using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiWidget;
    [SerializeField] private int money;
    
    public int Money
    {
        get => money;
        set
        {
            money = value;
            UpdateDisplay();
        }
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        uiWidget.text = "$" + money.ToString();
    }
}
