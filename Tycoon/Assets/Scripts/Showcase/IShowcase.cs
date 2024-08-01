using System;
using UnityEngine;
public interface IShowcase
{

    event Action BuyerHasGoneSignal;
    Transform FirstPointOfQueue { get; set; }
    int PplInQueueAmount 
    {
        get
        {
            return PplInQueueAmount;
        }
        set
        {
            if (value == 0)
                PplInQueueAmount = 1;
        }
    }

    void Subscribe2BuyerGoingSignal(Action action, ref int numerationOfBuyerInQueue)
    {
        
        Debug.Log("SubscribeMovingForwardInQueue");
        BuyerHasGoneSignal += action;
        numerationOfBuyerInQueue = PplInQueueAmount;
        PplInQueueAmount++;
    }
    void Unsubscribe2BuyerGoingSignal(Action action)
    {
        Debug.Log("UnSubscribeMovingForwardInQueue");
        BuyerHasGoneSignal -= action;
        PplInQueueAmount--;
    }

}
