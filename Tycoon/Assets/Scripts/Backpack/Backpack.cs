using System.Collections.Generic;
using UnityEngine;

public abstract class Backpack : MonoBehaviour
{

    private List<Item> _items = new List<Item>();
    private int _currentBackpackCapacity;
    [SerializeField] private int _maxBackpackCapacity;
    [SerializeField] private Transform _itemStartPosition;


    public void SaveItem(Item item)
    {
        if (_currentBackpackCapacity < _maxBackpackCapacity)
        {
        _currentBackpackCapacity++;
        _items.Add(item);
            item.transform.parent = gameObject.transform;
            item.transform.position = Vector3.zero;
            SortItem();
        }

    }

    private void SortItem()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].gameObject.transform.position = _itemStartPosition.position + new Vector3(0, i-0f, 0);
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
    public bool CheckBackpackCapacity()
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
