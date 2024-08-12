using System;
using System.Collections;
using System.Collections.Generic;
using Tycoon.PlayerSystems;
using UnityEngine;
using Zenject;

public class TimeEditor : MonoBehaviour
{
    private bool _activated;
    [Inject] private void Construct(PlayerHotkeys playerHotkeys)
    {
        playerHotkeys.TkeyPressed += TkeyPressed;
    }

    private void TkeyPressed()
    {
        if (!_activated)
        {
            gameObject.SetActive(true);
            _activated = true;
        }
        else
        {
            gameObject.SetActive(false);
            _activated = false;
        }
    }
}
