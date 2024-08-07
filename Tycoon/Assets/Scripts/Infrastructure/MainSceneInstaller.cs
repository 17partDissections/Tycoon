using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Tycoon.Factories;

public class MainSceneInstaller : MonoInstaller, IInitializable
{
    [SerializeField] private Wallet _walletInstance;
    [SerializeField] private List<BuyerStateMachine> _buyers;

    public void Initialize()
    {
        var buyersObjectPool = Container.Resolve<ObjectPool<BuyerStateMachine>>();
        buyersObjectPool.InitPool(Container.TryResolveId<Tycoon.Factories.IFactory>(typeof(BuyersFactory)), 5);
    }
    public override void InstallBindings()
    {
        BindMainInstllaersIntarface();
        BindBuyersObjectPool();
        BindBuyersFactory();
        BindListOfBuyers();
        BindEventBus();
        BindStorage();
        BindWallet();
        BindQueueHandler();
    }

    private void BindQueueHandler()
    {
        Container
            .Bind<QueueHandler>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }

    private void BindMainInstllaersIntarface()
    {
        Container.BindInterfacesTo<MainSceneInstaller>()
            .FromInstance(this)
            .AsSingle()
            .NonLazy();
    }
    private void BindBuyersObjectPool()
    {
        Container.Bind<ObjectPool<BuyerStateMachine>>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
    private void BindBuyersFactory()
    {

        Container.Bind<Tycoon.Factories.IFactory>()
                    .WithId(typeof(BuyersFactory))
                    .To<BuyersFactory>()
                    .FromNew()
                    .AsSingle()
                    .NonLazy();
    }
    private void BindListOfBuyers()
    {
        Container.Bind<List<BuyerStateMachine>>()
            .FromInstance(_buyers)
            .AsSingle()
            .NonLazy();
    }

    private void BindEventBus()
    {
        Container
                    .Bind<EventBus>()
                    .FromNew()
                    .AsSingle()
                    .NonLazy();
    }
    private void BindStorage()
    {
        Container
            .Bind<Storage>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
    private void BindWallet()
    {
        Container
                    .Bind<Wallet>()
                    .FromInstance(_walletInstance)
                    .AsSingle()
                    .NonLazy();
    }
}
