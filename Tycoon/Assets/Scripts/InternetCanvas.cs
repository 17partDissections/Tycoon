using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetCanvas : MonoBehaviour
{
    public bool Activated;
    [SerializeField] private GameObject _internetObj;
    [SerializeField] private GameObject _gamepad;

    public void OpenInternet()
    {
        Activated = true;
        Time.timeScale = 0;
        _gamepad.SetActive(false);
        _internetObj.SetActive(true);
        
    }
    public void CloseInternet()
    {
        Activated = false;
        Time.timeScale = 1;
        _gamepad.SetActive(false);
        _internetObj.SetActive(false);
    }
}
