using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class RewardedAd : MonoBehaviour
{
    private bool _isPlayerInTrigger;
    private FabricsNShowcasesCanvas _canvas;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _buyCircle;

    private void Awake() { _canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, 0, _buyCircle); _canvas.OnlyIcon(); }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _isPlayerInTrigger = true;

    }
    private void OnTriggerStay(Collider other) { Process(); }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _canvas.BuyCircle.fillAmount = 0;
            _isPlayerInTrigger = false;
        }
    }
    private void Process()
    {
        if (_canvas.BuyCircle.fillAmount < 1)
            _canvas.BuyCircle.fillAmount += Time.deltaTime;
        else
        {
            _canvas.BuyCircle.fillAmount = 0;
            InvokeRewardedAd();
        }
    }
    public void InvokeRewardedAd() { YandexGame.RewardVideoEvent.Invoke(0); }
}
