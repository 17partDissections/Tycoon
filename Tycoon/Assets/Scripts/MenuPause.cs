using UnityEngine;
using Zenject;

public class MenuPause : MonoBehaviour
{


    private bool _activated;
    [SerializeField] private GameObject _menuPanel;
    //[SerializeField] private GameObject _main;
    //[SerializeField] private GameObject _settings;
    //[SerializeField] private GameObject _rUSur;
    private InternetCanvas _internetCanvas;

    [Inject] private void Construct(InternetCanvas internet)
    {
        _internetCanvas = internet;
    }
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape)) { if (!_activated) Activate(); else Deactivate(); }
    //}
    public void Activate()
    {
        //_main.SetActive(true);
        //_settings.SetActive(false);
        //_rUSur.SetActive(false);
        _menuPanel.SetActive(true);
        Time.timeScale = 0;
        _activated = true;
    }
    public void Deactivate()
    {
        _menuPanel.SetActive(false);
        if (!_internetCanvas.Activated)
            Time.timeScale = 1;
        _activated = false;
    }


}
