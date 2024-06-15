using System.Collections.Generic;
using UnityEngine;

public abstract class Backpack : MonoBehaviour
{

    protected List<Item> _items = new List<Item>();
    private int _currentBackpackCapacity;
    [SerializeField] private int _maxBackpackCapacity;
    [SerializeField] private Transform _itemStartPosition;


    public void SaveItem(Item item)
    {
        if (_currentBackpackCapacity < _maxBackpackCapacity)
        {
        _currentBackpackCapacity++;
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
                _currentBackpackCapacity--;
                Destroy(item.gameObject);
                _items.Remove(item);
                SortItem();
                return item;
            }
        }
        return null;

    }
    public bool IsBackpackNotFull()
    {
        if(_currentBackpackCapacity != _maxBackpackCapacity)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}
