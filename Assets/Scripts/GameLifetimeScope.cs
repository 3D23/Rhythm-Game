using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //Entry Points Register
        builder.UseEntryPoints(builder =>
        {
            builder.Add<Bootstrapper>();
        });

        builder.Register<MoneySaver>(Lifetime.Singleton)
            .As<ISaver>()
            .AsSelf();
        builder.Register<PlayerDataLoader>(Lifetime.Singleton)
            .As<ILoader>()
            .AsSelf();
        builder.Register<BinaryDataRepository>(Lifetime.Singleton)
            .As<IGameDataRepository<PlayerData, PlayerData.PlayerDataFields>>();
    }
}
