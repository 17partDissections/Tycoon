using UnityEngine;

namespace Tycoon.Factories
{
    public interface IFactory
    {
        GameObject Create();
        GameObject Create<T>() where T : MonoBehaviour;
    }
    

}