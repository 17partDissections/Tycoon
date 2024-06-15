using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GrowSignalCatch : MonoBehaviour
{
    private FabricAbstraction _fabricAbstraction;
    [SerializeField] GameObject _seedStageObj;
    [SerializeField] int _growStage;
    [SerializeField] private bool gOSA_F;
    [SerializeField] private bool gOSA_T;
    [SerializeField] private bool isItShouldBeDeactivated;

    private void Start()
    {
       _fabricAbstraction = GetComponentInParent<FabricAbstraction>();
       _fabricAbstraction.GrowSignal += ActivateStage;
    }
    private void ActivateStage(int stage)
    {
        if (stage == _growStage)
        {
            if (gOSA_F)
                _seedStageObj.SetActive(false);
            if (gOSA_T)
                _seedStageObj.SetActive(true);
        }
        if (isItShouldBeDeactivated)
        {
            if (stage - 2 == _growStage)
                _seedStageObj.SetActive(false);
        }
    }
}
