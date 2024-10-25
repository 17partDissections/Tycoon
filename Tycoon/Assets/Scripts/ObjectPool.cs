using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private List<T> _gameObjectsList;
    private bool _isBuyersPool;
    protected Tycoon.Factories.IFactory _factory;


    public ObjectPool(Tycoon.Factories.IFactory factory, int startCountOfObjects, bool isBuyersPool)
    {
        _isBuyersPool = isBuyersPool;
        _gameObjectsList = new List<T>();
        _factory = factory;
        for (int i = 0; i < startCountOfObjects; i++)
            CreateGameObjectForPool();
    }
    public void RemoveAllObject()
    {
        var gameObjecs = _gameObjectsList.FindAll(x => x.isActiveAndEnabled);
        foreach (var gameObj in gameObjecs)
        {
            DropBackToPool(gameObj);
        }
    }

    public T GetFromPool()
    {
        var gameObject = _gameObjectsList.FirstOrDefault(x => !x.isActiveAndEnabled);
        if (gameObject == null)
        {
            gameObject = CreateGameObjectForPool();
        }
        gameObject.transform.parent = null;
        gameObject.transform.position = new Vector3(0, -50, 0);
        gameObject.gameObject.SetActive(true);
        return gameObject;
    }

    public void DropBackToPool(T gameObject) => gameObject.gameObject.SetActive(false);

    private T CreateGameObjectForPool()
    {
        GameObject prefabGameObject = null;
        if (_isBuyersPool)
            prefabGameObject = _factory.Create();
        else
            prefabGameObject = _factory.Create<T>();
        prefabGameObject.SetActive(false);
        prefabGameObject.transform.position = new Vector3(0, -50, 0);
        _gameObjectsList.Add(prefabGameObject.GetComponent<T>());
        return prefabGameObject.GetComponent<T>();
    }
}
