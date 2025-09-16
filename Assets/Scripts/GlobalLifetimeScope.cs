using VContainer;
using VContainer.Unity;

public class GlobalLifetimeScope : LifetimeScope
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        //Entry Points Register
        builder.UseEntryPoints(builder =>
        {
            builder.Add<GameBootstrapper>();
        });

        builder.Register<MoneySetter>(Lifetime.Singleton)
            .As<IGameDataSetter<int>>()
            .AsSelf();
        builder.Register<PlayerDataLoader>(Lifetime.Singleton)
            .As<IGameDataLoader>()
            .AsSelf();
        builder.Register<PlayerDataSaver>(Lifetime.Singleton)
            .As<IGameDataSaver>()
            .AsSelf();
        builder.Register<BinaryDataRepository>(Lifetime.Singleton)
            .As<IGameDataRepository<PlayerData, PlayerData.PlayerDataFields>>();
        builder.Register<MoneyManager>(Lifetime.Singleton)
            .As<GameDataManager<int>>()
            .AsSelf();
    }
}
