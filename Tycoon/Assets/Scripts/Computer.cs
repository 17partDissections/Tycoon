using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using Zenject;

public class Computer : MonoBehaviour
{
    public int ComputerPrice;
    private bool Buyed;
    private InternetCanvas _internetCanvas;
    [SerializeField] private GameObject _computerVisual;
    private FabricsNShowcasesCanvas _canvas;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _buyCircle;
    private Wallet _playerWallet;
    private AudioHandler _audioHandler;
    private AudioClip _purchase;
    private SaveSystem _saveSystem;

    [Inject] private void Construct(InternetCanvas internet, Wallet wallet, AudioHandler audioHandler, SaveSystem saveSys)
    {
        _internetCanvas = internet;
        _playerWallet = wallet;
        _audioHandler = audioHandler;
        _saveSystem = saveSys;
        _canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, ComputerPrice, _buyCircle);
    }
    private void Awake()
    {
        _canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, ComputerPrice, _buyCircle);
        _canvas.SetPrice();
    }
   
    private void OnTriggerStay(Collider other)
    {
        //if (other.tag == "Player")
        //    if (!Buyed)
        //        BuyingProcess();
        //    else
        //        OpeningInternetProcess();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _canvas.BuyCircle.fillAmount = 0;
    }

    private void BuyingProcess()
    {
        if (_canvas.BuyCircle.fillAmount < 1)
            _canvas.BuyCircle.fillAmount += Time.deltaTime;
        else
        {
            _canvas.BuyCircle.fillAmount = 0;
            BuyComputer();
        }
    }
    public void BuyComputer()
    {
        Debug.Log(_playerWallet);
        if (_playerWallet.Trying2BuySmthng(ComputerPrice) == true)
        {
            Debug.Log("Buyed!");
            Buyed = true;
            _audioHandler.PlaySFX(_purchase);
            _canvas.OnlyIcon();
            _computerVisual.SetActive(true);
            //_saveSystem.SaveComputer(this);
        }
    }
    private void OpeningInternetProcess()
    {
        if (_canvas.BuyCircle.fillAmount < 1)
            _canvas.BuyCircle.fillAmount += Time.deltaTime / 2;
        else
        {
            _canvas.BuyCircle.fillAmount = 0;
            OpenInternet();
        }
    }
    private void OpenInternet()
    {
        _internetCanvas.OpenInternet();
    }
}
