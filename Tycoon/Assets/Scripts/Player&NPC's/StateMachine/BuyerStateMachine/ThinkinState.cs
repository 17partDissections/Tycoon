using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ThinkinState : BaseState<BuyerStateMachine.BuyerStates>
{
    private BuyerStateMachine _stateMachine;
    public ThinkinState(BuyerStateMachine.BuyerStates state,
        BuyerStateMachine CurrentStateMachine) : base(state)
    {
        _stateMachine = CurrentStateMachine;
    }

    public override void Enter2State()
    {
        ItemName[] list = _stateMachine.Storage.GetAviableItems();
        for (int i = 0; i < _stateMachine.WannaBuy.Length; i++)
        {
            _stateMachine.WannaBuy[i] = list[RandomizeItem(list)];
        }
        _stateMachine.BackpackBuyer.WannaBuy.Clear();
        _stateMachine.BackpackBuyer.WannaBuy.AddRange(_stateMachine.WannaBuy);
        ChangeStateAction(BuyerStateMachine.BuyerStates.Going2ItemState);

    }

    public override void Exit2State()
    {

    }

    public override void UpdateState()
    {
       

    }
    private int RandomizeItem(ItemName[] list)
    {
        int randomNumber = Random.Range(0, list.Length);
        return randomNumber;    
    }
}
