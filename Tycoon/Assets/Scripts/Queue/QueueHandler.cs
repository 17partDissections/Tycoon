using System.Collections.Generic;
using UnityEngine;

public class QueueHandler
{
    public Dictionary<ItemName, List<BuyerStateMachine>> BuyerQueueDictionary = new Dictionary<ItemName, List<BuyerStateMachine>>();


    public void MoveBuyersInQueue(ItemName itemName)
    {
        foreach (var buyer in BuyerQueueDictionary[itemName])
            MovingForwardInQueue(buyer);
    }

    public void AddBuyerToQueue(BuyerStateMachine buyerStateMachine, ItemName itemName, IShowcase showcase)
    {
        if (!BuyerQueueDictionary.ContainsKey(itemName))
        {
            BuyerQueueDictionary.Add(itemName, new List<BuyerStateMachine>() { buyerStateMachine });
            buyerStateMachine.NumerationOfBuyerInQueue = showcase.PplInQueueAmount;
            showcase.PplInQueueAmount++;
        }
        else
        {
            BuyerQueueDictionary[itemName].Add(buyerStateMachine);
            buyerStateMachine.NumerationOfBuyerInQueue = showcase.PplInQueueAmount;
            showcase.PplInQueueAmount++;
        }
    }
    public void RemoveBuyerFromQueue(BuyerStateMachine buyerStateMachine, ItemName itemName, IShowcase showcase)
    {
        if (BuyerQueueDictionary.ContainsKey(itemName))
        {
            BuyerQueueDictionary[itemName].Remove(buyerStateMachine);
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
