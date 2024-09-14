using System.Collections;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using Zenject;

public abstract class FabricAbstraction : MonoBehaviour, IBuyable
{
    public int FabricPrice;

    [SerializeField] protected Item Item;
    [SerializeField] private GameObject _tilliage;
    [SerializeField] protected GameObject VisualTemplate;
    [SerializeField] private bool _isItHaveTilliage = true;
    protected Item ItemCopy;
    [SerializeField] private int _growSpeed;
    protected int GrowSpeed => _growSpeed;
    public Action<int> GrowSignal;
    protected int Timer;
    public int CurrentGrowSecond => Timer;
    private bool _isPlayerInTrigger;



    public bool CanGrab;
    private bool _isCoroutineStarted;
    protected bool Buyed;
    protected Coroutine HashCoroutine;

    protected FabricsNShowcasesCanvas Canvas;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _buyCircle;
    [SerializeField] private TextMeshProUGUI _text;
    private EventBus _eventbus;
    [SerializeField] private int _stage;
    private Wallet _playerWallet;
    protected AudioHandler AudioHandler;
    [SerializeField] private AudioClip _purchase;
    [SerializeField] protected AudioClip PlantRemoving;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _isPlayerInTrigger = true;

        if (Buyed)
        {
            if (CanGrab)
            {
                other.TryGetComponent<Backpack>(out Backpack backpack);
                if (backpack is BackpackWorker && !backpack.IsBackpackFull())
                {
                    AudioHandler.PlaySFX(PlantRemoving);
                    backpack?.SaveItem(ItemCopy);
                    CanGrab = false;
                    Canvas.Text.text = "";
                    GrowSignal.Invoke(Timer += 2); //disabling last phase visual object
                    if (HashCoroutine == null)
                    {
                        HashCoroutine = StartCoroutine(GrowCoroutine());
                    }

                }
                else
                {
                    Debug.Log("Backpack is full");
                }
            }
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (Buyed)
        {
            if (CanGrab)
            {
                other.TryGetComponent<Backpack>(out Backpack backpack);
                if (backpack is BackpackWorker && !backpack.IsBackpackFull())
                {
                    AudioHandler.PlaySFX(PlantRemoving);
                    backpack?.SaveItem(ItemCopy);
                    CanGrab = false;
                    Canvas.Text.text = "";
                    GrowSignal.Invoke(Timer += 2); //disabling last phase visual object
                    if (HashCoroutine == null)
                        HashCoroutine = StartCoroutine(GrowCoroutine());
                }
            }
        }
        else
        {
            BuyingProcess();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
                Canvas.BuyCircle.fillAmount = 0;
                _isPlayerInTrigger = false;
           
        }
    }

    [Inject]
    private void Construct(EventBus bus, Wallet wallet, AudioHandler audioSources)
    {
        _eventbus = bus;
        _playerWallet = wallet;
        AudioHandler = audioSources;
    }
    protected virtual IEnumerator GrowCoroutine()
    {
        Timer = 0;
        while (Timer < _growSpeed)
        {
            yield return new WaitForSeconds(1);
            Timer++;
            GrowSignal.Invoke(Timer);
            Canvas.Text.text = (Timer + "/" + _growSpeed).ToString();

        }
        Canvas.Text.text = "done";
        var itemCopyOfGameObject = Instantiate(VisualTemplate, gameObject.transform);
        itemCopyOfGameObject.TryGetComponent(out Item itemCopy);
        if (itemCopy != null)
        {
            ItemCopy = itemCopy;
        }
        CanGrab = true;
        HashCoroutine = null;
    }

    private void Start()
    {
        Canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, FabricPrice, _buyCircle);

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
            Buyed = true;
            AudioHandler.PlaySFX(_purchase);
            Canvas.OnlyIcon();
            gameObject.SetActive(true);
            if (_isItHaveTilliage)
                _tilliage.SetActive(true);
            _eventbus.StageSignal.Invoke(_stage);
            if (_isCoroutineStarted == false)
            {
                HashCoroutine = StartCoroutine(GrowCoroutine());
                _isCoroutineStarted = true;
            }

        }

    }

    public void BuyingProcess()
    {

        if (Canvas.BuyCircle.fillAmount < 1)
            Canvas.BuyCircle.fillAmount += Time.deltaTime;
        else
        {
            Canvas.BuyCircle.fillAmount = 0;
            BuyFabric();
        }


    }
}
