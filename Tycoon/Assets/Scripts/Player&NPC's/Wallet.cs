using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int CoinsAmount;
    private TextMeshProUGUI _visualCoins;
    [Range(0, 160)] [SerializeField] private int _startCoinsAmount;

    void Start()
    {
        CoinsAmount = _startCoinsAmount;
        _visualCoins = GetComponentInChildren<TextMeshProUGUI>();
        _visualCoins.text = CoinsAmount.ToString();
    }

    public bool Trying2BuySmthng(int price)
    {
       if(CoinsAmount - price >= 0)
        {
            CoinsAmount -= price;
            _visualCoins.text = CoinsAmount.ToString();
            return true;
        }
       else
        {
            return false;
        }
    }
}
