//using System.Collections;
//using UnityEngine;

//public class BuySign : MonoBehaviour
//{
//    [SerializeField] private FabricAbstraction _fabric;
//    private int _timer = 2;

//    private void OnTriggerStay(Collider other)
//    {
//        // if достаточно ли денег
//        _fabric._buyCircle.enabled = true;
//        while (_timer < 3)
//        {
//            //_timer++;
//            //Debug.Log(_timer);
//            //_fabric._buyCircle.fillAmount = 1 * _timer;
//        }
//        _fabric._buyCircle.enabled = false;
//        _fabric.BuyFabric();

//        Destroy(gameObject);
//    }
//    private void OnTriggerExit(Collider other)
//    {
//        _timer = 0;
//    }
//}
