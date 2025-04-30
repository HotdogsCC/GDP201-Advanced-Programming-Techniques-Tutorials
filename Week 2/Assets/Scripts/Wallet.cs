using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI uiWidget;
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

    public void SaveWallet(string saveFilePath)
    {
        string moneyAsString = Money.ToString();
        System.IO.File.WriteAllText(saveFilePath, moneyAsString);
    }

    public void LoadWallet(string saveFilePath)
    {
        // check the file exists
        if (System.IO.File.Exists(saveFilePath))
        {
            //read the file
            string txt = System.IO.File.ReadAllText(saveFilePath);
            
            //try to convert the string to an integer
            int.TryParse(txt, out int moneyFromTxt);

            //value wont be -1 if parsed without errors
            if (moneyFromTxt != -1)
            {
                Money = moneyFromTxt;
            }
        }
    }
}
