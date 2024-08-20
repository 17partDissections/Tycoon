using System;
using System.Collections;
using Tycoon.Factories;
using UnityEngine;
using Zenject;

public class BuyersHandler : MonoBehaviour
{
    private ObjectPool<BuyerStateMachine> _objectPool;
    private WaitForSeconds _delay = new WaitForSeconds(2);
    public int _buyerClonesOnScene;
    public int MaxAmountOfBuyerClones;
    [SerializeField] private int _stage;
    
    [Inject] private void Construct(ObjectPool<BuyerStateMachine> objectPool, EventBus eventBus)
    {
        _objectPool = objectPool;
        eventBus.BuyerGoAwaySignal += () => _buyerClonesOnScene--;
        eventBus.StageSignal += Check4Stage;
    }

    private void Check4Stage(int stage)
    {
        if(stage == _stage) 
        {
            StartCoroutine(AddBuyerCoroutine());
        }
    }

    private IEnumerator AddBuyerCoroutine()
    {
        while (true)
        {
            if (_buyerClonesOnScene < MaxAmountOfBuyerClones)
            {
                yield return _delay;
                var buyer = _objectPool.GetFromPool();
                buyer.ObjectPool = _objectPool;
                buyer.ChangeStateFromMachine(BuyerStateMachine.BuyerStates.StartState);
                _buyerClonesOnScene++;
            }
            else
            {
                yield return _delay;

            }

        }

    }
}
