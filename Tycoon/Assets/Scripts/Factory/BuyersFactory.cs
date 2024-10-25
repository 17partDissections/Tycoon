using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tycoon.Factories
{
    public class BuyersFactory : IFactory
    {

        private List<BuyerStateMachine> _buyersList = new List<BuyerStateMachine>();
        private DiContainer _container;
        public BuyersFactory(List<BuyerStateMachine> buyersList, DiContainer diContainer)
        {
            _buyersList = buyersList;
            _container = diContainer;
        }
        public GameObject Create()
        {
            var randomIndex = Random.Range(0, _buyersList.Count);
            GameObject buyer = Object.Instantiate(_buyersList[randomIndex]).gameObject;
            _container.Inject(buyer.GetComponent<BuyerStateMachine>());
            //buyer.GetComponent<BuyerStateMachine>().Init(_container.Resolve<Storage>());
            return buyer;
        }

        public GameObject Create<T>() where T : MonoBehaviour
        {
            throw new System.NotImplementedException();
        }
    }
}