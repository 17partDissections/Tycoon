using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MenuPause : MonoBehaviour
{


    private bool _activated;
    [SerializeField] private GameObject _menuPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { if (!_activated) Activate(); else Deactivate(); }
    }
    private void Activate()
    {
        _menuPanel.SetActive(true);
        Time.timeScale = 0;
        //Time.unscaledTime = 0; 
        _activated = true;
    }
    private void Deactivate()
    {
        _menuPanel.SetActive(false);
        _activated = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (materials.bounds.Intersects(other.bounds))
    //    {
    //        Debug.Log("ih8ny");

    //    }
    //    else if (donePorduction.bounds.Intersects(other.bounds)) { Debug.Log("123123123)"); };
    //}


}
