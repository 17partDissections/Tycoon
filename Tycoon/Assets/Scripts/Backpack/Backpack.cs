using System.Collections.Generic;
using UnityEngine;

public abstract class Backpack : MonoBehaviour
{

    public List<Item> Items = new List<Item>();
    [SerializeField] private int _maxBackpackCapacity;
    [SerializeField] private Transform _itemStartPosition;

    public void SaveItem(Item item)
    {
        if (Items.Count < _maxBackpackCapacity)
        {
        Items.Add(item);
            item.gameObject.SetActive(true);
            item.transform.rotation = Quaternion.identity;
            item.transform.rotation = Quaternion.Euler(-90, 0, 0);
            item.transform.parent = _itemStartPosition;
            item.transform.position = Vector3.zero;
            SortItem();
        }
        

    }

    private void SortItem()
    {
        float height = 0;
        for (int i = 0; i < Items.Count; i++)
        {
            if (i == 0)
            {
                Items[i].gameObject.transform.position = _itemStartPosition.position + new Vector3(0, i, 0);
            height = i;
            }
            else
            {
                Items[i].gameObject.transform.position = _itemStartPosition.position + new Vector3(0, height +0.5f, 0);
                height += 0.5f;
            }


        }
    }


    public Item RemoveItem(ItemName itemType)
    {
        foreach (var item in Items)
        {
            if (item.ItemName == itemType)
            {
                Destroy(item.gameObject);
                Items.Remove(item);
                SortItem();
                return item;
            }
        }
        return null;

    }
    public void DestroyAllItems()
    {
        foreach (var item in Items)
        {
            Destroy(item.gameObject);

        }
        Items.Clear();
    }

    public List<Item> GiveItemsList()
    {
        return Items;
    }
    public bool IsBackpackFull()
    {
        if(Items.Count == _maxBackpackCapacity)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}
