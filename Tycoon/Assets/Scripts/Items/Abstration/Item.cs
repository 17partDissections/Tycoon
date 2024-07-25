using UnityEngine;

public abstract class Item : MonoBehaviour
{
   protected int _price;
    public int Price => _price;
   [SerializeField] private ItemName _itemName;
    public ItemName ItemName => _itemName;
}
