using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private int _coinsAmount;
    public int CoinsAmount
    {
        get
        {
            return _coinsAmount;
        }
        set
        {
            _coinsAmount = value;
            _addMoneySignal?.Invoke();
        }
    }
    [Range(0, 160)] [SerializeField] private int _startCoinsAmount;
    private TextMeshProUGUI _visualCoins;
    private Action _addMoneySignal;


    void Start()
    {
        CoinsAmount = _startCoinsAmount;
        _visualCoins = GetComponentInChildren<TextMeshProUGUI>();
        PrintMoney();
        _addMoneySignal += PrintMoney;
    }

    private void PrintMoney()
    {
        _visualCoins.text = _coinsAmount.ToString();
    }

    public bool Trying2BuySmthng(int price)
    {
       if(CoinsAmount - price >= 0)
        {
            CoinsAmount -= price;
            PrintMoney();
            return true;
        }
       else
        {
            return false;
        }
    }
}
