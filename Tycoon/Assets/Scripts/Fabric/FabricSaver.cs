using UnityEngine;
using Zenject;
using System.Collections.Generic;


public class FabricSaver : MonoBehaviour
{
    
    public List<Transform> _fabrics;
    private LoadSystem _loadSystem;
    [Inject]
    private void Construct(SaveSystem saveSys, LoadSystem loadSys)
    {
        _loadSystem = loadSys;
    }
    private void Start()
    {
        foreach (Transform ft in _fabrics)
        {
            _loadSystem.LoadFabric(ft);
        }

    }
}
