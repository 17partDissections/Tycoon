using UnityEngine;
using Zenject;

public class ItemHandler : MonoBehaviour
{
    [InjectOptional] public ObjectPool<Strawberry> StrawberryPool;
    [InjectOptional] public ObjectPool<Banana> BananaPool;
    [InjectOptional] public ObjectPool<Lemon> LemonPool;
    [InjectOptional] public ObjectPool<StrawberryJam> StrawberryJamPool;
    [InjectOptional] public ObjectPool<Lemonade> LemonadePool;
    [InjectOptional] public ObjectPool<Watermelon> WatermelonPool;
}
