using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage 
{
    private Dictionary<ItemName, Transform> _aviableItems = new Dictionary<ItemName, Transform>();
    public Transform CashierPosition;

    public void AddItem2Aviable(ItemName itemName, Transform transform)
    {
        _aviableItems.Add(itemName, transform);
    }
    public ItemName[] GetAviableItems()
    {


        return _aviableItems.Keys.ToArray();
    }
    public Transform GetPosition(ItemName item)
    {
        return _aviableItems[item];
    }
    public void AddCashierPosition(Transform cashierTransform)
    {
         CashierPosition = cashierTransform;
    }
}
