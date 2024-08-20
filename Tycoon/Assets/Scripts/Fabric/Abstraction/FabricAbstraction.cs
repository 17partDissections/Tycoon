using System.Collections;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using Zenject;

public abstract class FabricAbstraction : MonoBehaviour
{
    public int FabricPrice;

    [SerializeField] private Item _item;
    [SerializeField] private GameObject _tilliage;
    [SerializeField] private GameObject _visualTemplate;
    [SerializeField] private bool _isItHaveTilliage = true;
    private Item _itemCopy;
    [SerializeField] private int _growSpeed;
    public Action<int> GrowSignal;
    private int _timer;
    private bool _canGrab;
    private bool _isCoroutineStarted;
    private bool _buyed;

    private FabricsNShowcasesCanvas _canvas;
    [SerializeField] private Image _itemIcon;
    public Image _buyCircle;
    [SerializeField] private TextMeshProUGUI _text;
    private EventBus _eventbus;
    [SerializeField] private int _stage;
    private Wallet _playerWallet;
    private AudioHandler _audioHandler;
    [SerializeField] private AudioClip _purchase;
    [SerializeField] private AudioClip _plantRemoving;

    private void OnTriggerEnter(Collider other)
    {
        if (_buyed)
        {
            if (_canGrab)
            {
                other.TryGetComponent<Backpack>(out Backpack backpack);
                if (backpack is BackpackWorker && !backpack.IsBackpackFull())
                {
                    _audioHandler.PlaySFX(_plantRemoving);
                    backpack?.SaveItem(_itemCopy);
                    _canGrab = false;
                    _canvas.Text.text = "";
                    GrowSignal.Invoke(_timer += 2); //disabling last phase visual object
                    _timer = 0;
                    StartCoroutine(GrowCoroutine());
                }
                else
                {
                    Debug.Log("Backpack is full");
                }
            }
        }
        else
        {
            BuyFabric();
        }

    }

    [Inject]
    private void Construct(EventBus bus, Wallet wallet, AudioHandler audioSources)
    {
        _eventbus = bus;
        _playerWallet = wallet;
        _audioHandler = audioSources;
    }
    private IEnumerator GrowCoroutine()
    {
        while (_timer < _growSpeed)
        {
            yield return new WaitForSeconds(1);
            _timer++;
            GrowSignal.Invoke(_timer);
            _canvas.Text.text = (_timer + "/" + _growSpeed).ToString();

        }
        _canvas.Text.text = "done";
        var itemCopyOfGameObject = Instantiate(_visualTemplate, gameObject.transform);
        //itemCopyOfGameObject.SetActive(true);
        itemCopyOfGameObject.TryGetComponent(out Item itemCopy);
        if (itemCopy != null)
        {
            _itemCopy = itemCopy;
        }
        _canGrab = true;
    }

    private void Start()
    {
        _canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, FabricPrice);
    }
    [ContextMenu("InitInspect")]
    public void InitInspector()
    {
        var parentObj = transform.parent;
        Debug.Log(parentObj.name);
        _itemIcon = parentObj.GetComponentInChildren<Image>();
        _text = parentObj.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void BuyFabric()
    {
        if (_playerWallet.Trying2BuySmthng(FabricPrice) == true)
        {
            _buyed = true;
            _audioHandler.PlaySFX(_purchase);
            _canvas.OnlyIcon();
            gameObject.SetActive(true);
            if(_isItHaveTilliage)
                _tilliage.SetActive(true);
            _eventbus.StageSignal.Invoke(_stage);
            if (_isCoroutineStarted == false)
            {
                StartCoroutine(GrowCoroutine());
                _isCoroutineStarted = true;
            }

        }

    }
}
