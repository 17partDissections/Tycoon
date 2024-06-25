using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Zenject;

public class BuyersFabric : MonoBehaviour, IFabric
{ 
    public List<BuyerStateMachine> _buyers;
    private DiContainer _container;

    public void Create()
    {
        
        _buyers[0].gameObject.SetActive(true);
        _buyers[0].Init(_container.Resolve<Storage>());
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.I))
        {
            Create();
        }
    }
    public void Init(DiContainer container)
    {
        _container = container;
    }
    }

