using UnityEngine;

public abstract class Item : MonoBehaviour
{
   [SerializeField] private int _cost;
   [SerializeField] private ItemName _itemName;
    public ItemName ItemName => _itemName;
}
