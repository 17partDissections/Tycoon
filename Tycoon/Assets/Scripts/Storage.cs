using System.Collections.Generic;
using UnityEngine;
public class Storage 
{
    private Dictionary<ItemName, Vector3> _positionInQueueOfItems = new Dictionary<ItemName, Vector3>();
    public Dictionary<ItemName, DirectionOfQueue> DirectionOfQueue = new Dictionary<ItemName, DirectionOfQueue>();
    public Dictionary<ItemName, IShowcase> IShowcaseDictionary = new Dictionary<ItemName, IShowcase>();

    public Transform CashierPosition;

    public void AddItem2Aviable(ItemName itemName, Vector3 vector3)
    {
        if (!_positionInQueueOfItems.ContainsKey(itemName))
        {
            _positionInQueueOfItems.Add(itemName, vector3);
        }
        else
        {
            _positionInQueueOfItems[itemName] = vector3;
        }

    }
    public ItemName[] GetAviableItems()
    {
        List<ItemName> items = new List<ItemName>();
        foreach (var item in _positionInQueueOfItems)
        {
            if (item.Key == ItemName.Cashier)
                continue;
            else
                items.Add(item.Key);
        }

        return items.ToArray();
    }
    public Vector3 GetPosition(ItemName item)
    {
        return _positionInQueueOfItems[item];
    }
    public void AddQueueDirection(ItemName item, DirectionOfQueue direction)
    {
        DirectionOfQueue.Add(item, direction);
    }
    //public void ChangePositionInQueue(ItemName item, bool AddNewBuyer)
    //{

    //    var lastPositionInQueue = _positionInQueueOfItems[item];
    //    if (AddNewBuyer)
    //    {
    //        IShowcaseDictionary[item].PplInQueueAmount++;
    //        switch (DirectionOfQueue[item])
    //        {
    //            case global::DirectionOfQueue.South:
    //                lastPositionInQueue += (Vector3.back);
    //                break;
    //            case global::DirectionOfQueue.North:
    //                lastPositionInQueue += (Vector3.forward);
    //                break;
    //            case global::DirectionOfQueue.West:
    //                lastPositionInQueue += (Vector3.left);
    //                break;
    //            case global::DirectionOfQueue.East:
    //                lastPositionInQueue += (Vector3.right);
    //                break;
    //        }
    //        _positionInQueueOfItems[item] = lastPositionInQueue;
    //    }
    //    else
    //    {
    //        //IShowcaseDictionary[item].PplInQueueAmount--;
    //        switch (DirectionOfQueue[item])
    //        {
    //            case global::DirectionOfQueue.South:
    //                lastPositionInQueue += (Vector3.forward);
    //                break;
    //            case global::DirectionOfQueue.North:
    //                lastPositionInQueue += (Vector3.back);
    //                break;
    //            case global::DirectionOfQueue.West:
    //                lastPositionInQueue += (Vector3.right);
    //                break;
    //            case global::DirectionOfQueue.East:
    //                lastPositionInQueue += (Vector3.left);
    //                break;
    //        }
    //        _positionInQueueOfItems[item] = lastPositionInQueue;
    //    }
    //}
    public void AddCashierPosition(Transform cashierTransform)
    {
         CashierPosition = cashierTransform;
    }

    public void AddShowcaseOrCashier(ItemName itemName, IShowcase iShowcase)
    {
        IShowcaseDictionary.Add(itemName, iShowcase);
    }
}
