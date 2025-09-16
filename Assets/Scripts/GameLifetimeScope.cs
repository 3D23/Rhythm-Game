using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //Entry Points Register
        builder.UseEntryPoints(builder =>
        {
            builder.Add<GameBootstrapper>();
        });

        builder.Register<MoneySetter>(Lifetime.Singleton)
            .As<ISetter<PlayerData.PlayerDataFields, int>>()
            .AsSelf();
        builder.Register<PlayerDataLoader>(Lifetime.Singleton)
            .As<ILoader>()
            .AsSelf();
        builder.Register<PlayerDataSaver>(Lifetime.Singleton)
            .As<ISaver>()
            .AsSelf();
        builder.Register<BinaryDataRepository>(Lifetime.Singleton)
            .As<IGameDataRepository<PlayerData, PlayerData.PlayerDataFields>>();
        builder.Register<MoneyManager>(Lifetime.Singleton)
            .As<GameManager<int>>()
            .AsSelf();
    }
}
