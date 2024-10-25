using System;
using System.Collections;
using Tycoon.Factories;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BuyersHandler : MonoBehaviour
{
    private ObjectPool<BuyerStateMachine> _objectPool;
    private WaitForSeconds _delay = new WaitForSeconds(2);
    public int _buyerClonesOnScene;
    public int MaxAmountOfBuyerClones;
    [SerializeField] private Transform _warpPos;

    
    [Inject] private void Construct(ObjectPool<BuyerStateMachine> objectPool, EventBus eventBus)
    {
        _objectPool = objectPool;
        eventBus.BuyerGoAwaySignal += () => _buyerClonesOnScene--;
        eventBus.StageSignal += Check4Stage;
    }

    private void Check4Stage(int stage)
    {
        if(stage == 2) 
        {
            StartCoroutine(AddBuyerCoroutine());
        }
        if (stage == 3)
        {
            MaxAmountOfBuyerClones++;
            StartCoroutine(AddBuyerCoroutine());
        }
        if (stage == 4)
        {
            MaxAmountOfBuyerClones++;
            StartCoroutine(AddBuyerCoroutine());
        }
        if (stage == 5)
        {
            MaxAmountOfBuyerClones++;
            StartCoroutine(AddBuyerCoroutine());
        }
        if (stage == 8)
        {
            MaxAmountOfBuyerClones+=2;
            StartCoroutine(AddBuyerCoroutine());
        }
        if (stage == 10)
        {
            MaxAmountOfBuyerClones++;
            StartCoroutine(AddBuyerCoroutine());
        }
        if (stage == 11)
        {
            MaxAmountOfBuyerClones++;
            StartCoroutine(AddBuyerCoroutine());
        }
    }

    private IEnumerator AddBuyerCoroutine()
    {
        while (true)
        {
            while (_buyerClonesOnScene < MaxAmountOfBuyerClones)
            {
                yield return _delay;
                if (_buyerClonesOnScene >= MaxAmountOfBuyerClones) { continue; }

                var buyer = _objectPool.GetFromPool();
                buyer.GetComponent<NavMeshAgent>().Warp(_warpPos.position);
                buyer.ChangeStateFromMachine(BuyerStateMachine.BuyerStates.StartState);
                _buyerClonesOnScene++;
            }
            yield return null;
        }

    }
}
