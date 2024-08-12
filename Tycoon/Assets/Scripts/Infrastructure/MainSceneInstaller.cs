using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Tycoon.Factories;
using System;
using Tycoon.PlayerSystems;

public class MainSceneInstaller : MonoInstaller, IInitializable
{
    [SerializeField] private Wallet _walletInstance;
    [SerializeField] private List<BuyerStateMachine> _buyers;
    [SerializeField] private PlayerHotkeys _player;
    [SerializeField] private AudioSources _audioSources;

    public void Initialize()
    {
        var buyersObjectPool = Container.Resolve<ObjectPool<BuyerStateMachine>>();
        buyersObjectPool.InitPool(Container.TryResolveId<Tycoon.Factories.IFactory>(typeof(BuyersFactory)), 5);
    }
    public override void InstallBindings()
    {
        BindMainInstallersInterface();
        BindPlayerHotkeys();
        BindBuyersObjectPool();
        BindBuyersFactory();
        BindListOfBuyers();
        BindEventBus();
        BindStorage();
        BindWallet();
        BindQueueHandler();
        BindAudioSources();
    }

    private void BindAudioSources()
    {
        Container
            .Bind<AudioSources>()
            .FromInstance(_audioSources)
            .AsSingle()
            .NonLazy();
    }

    private void BindPlayerHotkeys()
    {
        Container
            .Bind<PlayerHotkeys>()
            .FromInstance(_player)
            .AsSingle()
            .NonLazy();
    }

    private void BindQueueHandler()
    {
        Container
            .Bind<QueueHandler>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }

    private void BindMainInstallersInterface()
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
