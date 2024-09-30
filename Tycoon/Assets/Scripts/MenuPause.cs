using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MenuPause : MonoBehaviour
{


    private bool _activated;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _rUSur;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { if (!_activated) Activate(); else Deactivate(); }
    }
    private void Activate()
    {
        _main.SetActive(true);
        _settings.SetActive(false);
        _rUSur.SetActive(false);
        _menuPanel.SetActive(true);
        Time.timeScale = 0;
        _activated = true;
    }
    public void Deactivate()
    {
        _menuPanel.SetActive(false);
        Time.timeScale = 1;
        _activated = false;
    }


}
