using UnityEngine;
using Zenject;
using System.Collections.Generic;


public class ShowcaseSaver : MonoBehaviour
{
    public List<ShowcaseAbstraction> _showcases;
    private LoadSystem _loadSystem;
    [Inject]
    private void Construct(SaveSystem saveSys, LoadSystem loadSys)
    {
        _loadSystem = loadSys;
    }
    private void Start()
    {
        foreach (ShowcaseAbstraction sa in _showcases)
        {
           _loadSystem.LoadShowcase(sa);
            
        }

    }
}
