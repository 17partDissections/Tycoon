using System.Collections.Generic;
using UnityEngine;

public abstract class Backpack : MonoBehaviour
{

    public List<Item> _items = new List<Item>();
    [SerializeField] private int _maxBackpackCapacity;
    [SerializeField] private Transform _itemStartPosition;


    public void SaveItem(Item item)
    {
        if (_items.Count < _maxBackpackCapacity)
        {
        _items.Add(item);
            item.gameObject.SetActive(true);
            item.transform.parent = _itemStartPosition;
            item.transform.position = Vector3.zero;
            SortItem();
        }
        

    }

    private void SortItem()
    {
        float height = 0;
        for (int i = 0; i < _items.Count; i++)
        {
            if (i == 0)
            {
                _items[i].gameObject.transform.position = _itemStartPosition.position + new Vector3(0, i, 0);
            height = i;
            }
            else
            {
                _items[i].gameObject.transform.position = _itemStartPosition.position + new Vector3(0, height +0.5f, 0);
                height += 0.5f;
            }


        }
    }


    public Item RemoveItem(ItemName itemType)
    {
        foreach (var item in _items)
        {
            if (item.ItemName == itemType)
            {
                Destroy(item.gameObject);
                _items.Remove(item);
                SortItem();
                return item;
            }
        }
        return null;

    }
    public void DestroyAllItems()
    {
        foreach (var item in _items)
        {
            Destroy(item.gameObject);

        }
        _items.Clear();
    }

    public List<Item> GiveItemsList()
    {
        return _items;
    }
    public bool IsBackpackNotFull()
    {
        if(_items.Count != _maxBackpackCapacity)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}
