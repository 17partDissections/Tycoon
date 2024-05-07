using System.Collections;
using UnityEngine;

public abstract class FabricAbstraction : MonoBehaviour
{
    [SerializeField] private int _fabricCost;

    [SerializeField] private Item _item;
    [SerializeField] private GameObject _visualTemplate;
    private Item _itemCopy;
    [SerializeField] private int _growSpeed;
    private int _timer;
    private bool _canGrab;
    private bool _isCoroutineStarted;


    private void OnTriggerEnter(Collider other)
    {
        if (_canGrab)
        {
            
            other.TryGetComponent<Backpack>(out Backpack backpack);

            if (backpack is BackpackWorker && backpack.CheckBackpackCapacity())
            {
                backpack?.SaveItem(_itemCopy);
                //_visualTemplate.SetActive(false);
                _canGrab = false;
                StartCoroutine(GrowCoroutine()); 
            }
            else
            {
                Debug.Log("превышено максимальное количество объектов в рюкзаке");
            }
        }
    }

    private IEnumerator GrowCoroutine() 
    {
        while (_timer < _growSpeed)
        {
            yield return new WaitForSeconds(1);
            _timer++;
        }

        var itemCopyOfGameObject = Instantiate(_visualTemplate, gameObject.transform);
        itemCopyOfGameObject.SetActive(true);
        itemCopyOfGameObject.TryGetComponent(out Item itemCopy);
        if (itemCopy != null)
        {
            _itemCopy = itemCopy;
        }
        _canGrab = true;
    }

    private void Start()
    {
        BuyFabric();
    }
    public void BuyFabric()
    {
        gameObject.SetActive(true);
        if (_isCoroutineStarted == false)
        {
            StartCoroutine(GrowCoroutine());
            _isCoroutineStarted = true;
        }
    }
}
