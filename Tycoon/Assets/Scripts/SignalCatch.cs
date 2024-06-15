using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SignalCatch : MonoBehaviour
{
    [SerializeField] private int _stageID;
    private EventBus _eventbus;
    [SerializeField] private bool gOSA_F;
    [SerializeField] private bool gOSA_T;
    [SerializeField] private bool isItShouldBeDeactivated;
    [Inject] private void Construct(EventBus bus)
    {
        _eventbus = bus;
        _eventbus.StageSignal += ActivateStage;
    } 

    private void ActivateStage(int stage)
    {
        if (stage == _stageID)
        {
            if(gOSA_F)
                gameObject.SetActive(false);
            if (gOSA_T)
                gameObject.SetActive(true);
        }
        if (isItShouldBeDeactivated)
        {
        if(stage-1 == _stageID)
            gameObject.SetActive(false);
        }


    }
    
}
