using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySign : MonoBehaviour
{
    [SerializeField] private FabricAbstraction _fabric;

    private void OnTriggerEnter(Collider other)
    {
        // if ���������� �� �����
        _fabric.BuyFabric();
        Destroy(gameObject);
    }
}
