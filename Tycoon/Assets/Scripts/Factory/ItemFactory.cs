using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tycoon.Factories
{
    public class ItemFactory : IFactory
    {
        private List<Item> _itemsObjects = new List<Item>();
        public ItemFactory(List<Item> itemsObjects)
        {
            _itemsObjects = itemsObjects;
        }

        public GameObject Create()
        {
            throw new System.NotImplementedException();
        }

        public GameObject Create<T>() where T : MonoBehaviour
        {
            var neededItem = _itemsObjects.FirstOrDefault(x => x.GetType() == typeof(T)).gameObject;
            var gameObj = GameObject.Instantiate(neededItem);
            return gameObj;
        }
    }
}
