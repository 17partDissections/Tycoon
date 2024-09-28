using System;
using TMPro;
using UnityEngine;
using YG;

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
    [Range(0, 10000)] [SerializeField] private int _startCoinsAmount;
    private TextMeshProUGUI _visualCoins;
    private Action _addMoneySignal;


    void Start()
    {
        CoinsAmount = _startCoinsAmount;
        _visualCoins = GetComponentInChildren<TextMeshProUGUI>();
        //_coinsAmount = YandexGame.savesData.Money;
        _addMoneySignal += PrintMoney;
        PrintMoney();
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

    private void OnDestroy()
    {
        YandexGame.savesData.Money = _coinsAmount;
        YandexGame.SaveProgress();
    }
}
