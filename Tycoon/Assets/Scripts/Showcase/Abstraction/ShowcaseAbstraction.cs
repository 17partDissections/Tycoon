
using System.Collections.Generic;
using UnityEngine;

public abstract class ShowcaseAbstraction<T> : MonoBehaviour where T : Item
{
    [SerializeField] private List<T> _showcaseInventory;
    private T _item;
    private int _enabledItems4Buyers;


    private void AddItem()
    {

    }
    //private T RemoveItem()
    //{
    //    _enabledItems4Buyers--;
    //}

    private void Awake()
    {
        _item = _showcaseInventory[0];
    }
    private void OnTriggerEnter(Collider other)
    {






        if (other.TryGetComponent<Backpack>(out Backpack backpackAbstraction))
        {

            switch (backpackAbstraction.GetType().Name)
            {
                case nameof(BackpackWorker):
                    Debug.Log(_item.ItemName);
                    var trying2FindItem = backpackAbstraction.RemoveItem(_item.ItemName);
                    if (trying2FindItem != null)
                    {
                        var nonActiveItems = _showcaseInventory.FindAll(x => !x.isActiveAndEnabled);
                        int randomNumber = Random.Range(0, nonActiveItems.Count);
                        _enabledItems4Buyers++;
                        nonActiveItems[randomNumber].gameObject.SetActive(true);
                        Debug.Log(_enabledItems4Buyers);
                    }


                    break;
                case nameof(BackpackBuyer):


                    break;

            }
        }
    }


}
