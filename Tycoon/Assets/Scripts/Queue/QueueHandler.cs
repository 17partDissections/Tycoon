using System.Collections.Generic;
using UnityEngine;

public class QueueHandler
{
    public Dictionary<ItemName, List<BuyerStateMachine>> ByerQueueDictionary = new Dictionary<ItemName, List<BuyerStateMachine>>();


    public void MooveByersInQueue(ItemName itemName)
    {
        foreach (var byer in ByerQueueDictionary[itemName])
            MovingForwardInQueue(byer);
    }

    public void AddByerToQueue(BuyerStateMachine buyerStateMachine, ItemName itemName, IShowcase showcase)
    {
        if (!ByerQueueDictionary.ContainsKey(itemName))
        {
            ByerQueueDictionary.Add(itemName, new List<BuyerStateMachine>() { buyerStateMachine });
            buyerStateMachine.NumerationOfBuyerInQueue = showcase.PplInQueueAmount;
            showcase.PplInQueueAmount++;
        }
        else
        {
            ByerQueueDictionary[itemName].Add(buyerStateMachine);
            buyerStateMachine.NumerationOfBuyerInQueue = showcase.PplInQueueAmount;
            showcase.PplInQueueAmount++;
        }


    }
    public void RemoveByerFromQueue(BuyerStateMachine buyerStateMachine, ItemName itemName, IShowcase showcase)
    {
        if (ByerQueueDictionary.ContainsKey(itemName))
        {
            ByerQueueDictionary[itemName].Remove(buyerStateMachine);
            showcase.PplInQueueAmount--;
            buyerStateMachine.NumerationOfBuyerInQueue = 0;

        }

    }
    public Vector3 CalcPositionInQueue(DirectionOfQueue directionOfQueue, Transform firstPosition, int numerationInQueue)
    {
        Vector3 positionInQueue = firstPosition.position;
        if (numerationInQueue == 0)
            return positionInQueue;
        else
        {
            switch (directionOfQueue)
            {
                case DirectionOfQueue.South:
                    positionInQueue += (Vector3.back) * (numerationInQueue);
                    break;
                case DirectionOfQueue.North:
                    positionInQueue += (Vector3.forward) * (numerationInQueue);
                    break;
                case DirectionOfQueue.West:
                    positionInQueue += (Vector3.left) * (numerationInQueue);
                    break;
                case DirectionOfQueue.East:
                    positionInQueue += (Vector3.right) * (numerationInQueue);
                    break;
            }
            return positionInQueue;
        }
    }
    private void MovingForwardInQueue(BuyerStateMachine buyerStateMachine)
    {
        if (buyerStateMachine.NumerationOfBuyerInQueue > 0)
            buyerStateMachine.NumerationOfBuyerInQueue--;
        buyerStateMachine.
            Agent.SetDestination(CalcPositionInQueue(buyerStateMachine.Storage.DirectionOfQueue[buyerStateMachine.CurrentItemInList],
            buyerStateMachine.Storage.IShowcaseDictionary[buyerStateMachine.CurrentItemInList].FirstPointOfQueue, buyerStateMachine.NumerationOfBuyerInQueue));
    }
}
