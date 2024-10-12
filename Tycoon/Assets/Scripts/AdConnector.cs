using System.Collections;
using TMPro;
using UnityEngine;
using YG;
using Zenject;

public class AdConnector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private Wallet _wallet;

    [Inject] private void Construct(Wallet wallet)
    {
        _wallet = wallet;
        YandexGame.RewardVideoEvent += RewardedAd;
        //StartCoroutine(EveryMinuteAdCoroutine());

    }

    private void RewardedAd(int index)
    {
        if(index == 0)
            _wallet.CoinsAmount += 200;
    }



    private IEnumerator EveryMinuteAdCoroutine()
    {
        while (true)
        {
            var timer = 3;
            yield return new WaitForSeconds(10);
            _timerText.transform.parent.parent.gameObject.SetActive(true);
            _timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer -= 1;
            _timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer -= 1;
            _timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer -= 1;
            _timerText.text = timer.ToString();
            YandexGame.FullscreenShow();
            _timerText.transform.parent.parent.gameObject.SetActive(false);
        }
    }
   

}

