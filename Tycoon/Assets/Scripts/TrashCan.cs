using System;
using System.Collections;
using System.Collections.Generic;
using Tycoon.PlayerSystems;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHotkeys>(out  PlayerHotkeys hotkeys))
            if (other.TryGetComponent<Backpack>(out Backpack backpackAbstraction))
            {
                backpackAbstraction.DropBack2PoolAllItems();
            }
    }
}
