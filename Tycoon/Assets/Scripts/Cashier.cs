using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Cashier : MonoBehaviour
{
    [Inject] private void Construct(Storage storage)
    {
        storage.AddCashierPosition(gameObject.transform);
    }
}
