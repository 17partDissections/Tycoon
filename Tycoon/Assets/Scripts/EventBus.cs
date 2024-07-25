using System;

public class EventBus
{
    public Action<int> StageSignal;
    public Action<int> GrowSignal;
    public Action BuyerGoAwaySignal;
}

