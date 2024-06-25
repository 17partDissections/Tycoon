using Zenject;
using UnityEngine;

public class MainSceneInstaller : MonoInstaller, IInitializable
{
    [SerializeField] private Wallet _walletInstance;
    [SerializeField] private BuyersFabric _buyersFabric;

    public void Initialize()
    {
        _buyersFabric.Init(Container);
    }


    public override void InstallBindings()
    {
        BindMainInstllaersIntarface();

        BindEventBus();
        BindStorage();
        BindWallet();
    }

    private void BindMainInstllaersIntarface()
    {
        Container.BindInterfacesTo<MainSceneInstaller>()
            .FromInstance(this)
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
