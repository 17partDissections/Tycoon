using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindEventBus();
        BindStorage();
        BindWallet();
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
                    .FromInstance()
                    .AsSingle()
                    .NonLazy();
    }
}
