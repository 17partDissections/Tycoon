using System;
using UnityEngine;
public interface IShowcase
{

    event Action BuyerHasGoneSignal;
    Transform FirstPointOfQueue { get; set; }
    int PplInQueueAmount { get; set; }

    void Subscribe2BuyerGoingSignal(Action action, ref int numerationOfBuyerInQueue)
    {
        BuyerHasGoneSignal += action;
        numerationOfBuyerInQueue = PplInQueueAmount;
        //PplInQueueAmount++;
    }
    void Unsubscribe2BuyerGoingSignal(Action action)
    {
        BuyerHasGoneSignal -= action;
        PplInQueueAmount--;
    }

}
